using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using PoiNarration.Core.Models;
using PoiNarration.Mobile.Services;
using Location = Microsoft.Maui.Devices.Sensors.Location;

namespace PoiNarration.Mobile.Views;

public partial class MapPage : ContentPage
{
    private readonly AppDatabase _db;
    private readonly AutoBoothNavigatorService _autoBoothNavigatorService;
    private readonly GeofenceService _geofenceService;
    private Location? _lastMapCenterLocation;
    private DateTime _lastMapMoveUtc = DateTime.MinValue;


    private readonly Dictionary<Pin, Booth> _pinBoothMap = new();
    private List<Booth> _booths = new();
    private Booth? _currentNearestBooth;

    private bool _gpsModeEnabled = false;

    private readonly GpsModeStateService _gpsModeStateService;

    public MapPage(
        AppDatabase db,
        AutoBoothNavigatorService autoBoothNavigatorService,
        NarrationService narrationService,
        ApiService apiService,
        GeofenceService geofenceService,
        GpsModeStateService gpsModeStateService)
    {
        InitializeComponent();

        _db = db;
        _autoBoothNavigatorService = autoBoothNavigatorService;
        _geofenceService = geofenceService;
        _gpsModeStateService = gpsModeStateService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            await _db.InitAsync();

            _autoBoothNavigatorService.StateChanged -= OnAutoBoothStateChanged;
            _autoBoothNavigatorService.StateChanged += OnAutoBoothStateChanged;

            _gpsModeStateService.Changed -= OnGpsModeChanged;
            _gpsModeStateService.Changed += OnGpsModeChanged;

            _booths = await _db.GetAllBoothsAsync();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                BoothCountLabel.Text = $"{LanguageService.T("Ui_BoothCount")}: {_booths.Count}";
                LoadBoothPins();
            });

            await ApplyGpsModeAsync(_gpsModeStateService.IsEnabled);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(GetErrorTitleText(), ex.Message, GetOkText());
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _autoBoothNavigatorService.StateChanged -= OnAutoBoothStateChanged;
        _gpsModeStateService.Changed -= OnGpsModeChanged;
    }

    private void LoadBoothPins()
    {
        FoodMap.Pins.Clear();
        _pinBoothMap.Clear();

        foreach (var booth in _booths.Where(b => b.IsActive))
        {
            var pin = new Pin
            {
                Label = string.IsNullOrWhiteSpace(booth.NameVi) ? $"Booth {booth.Id}" : booth.NameVi,
                Address = booth.ZoneId,
                Type = PinType.Place,
                Location = new Location(booth.Lat, booth.Lng)
            };

            pin.MarkerClicked += OnPinMarkerClicked;
            FoodMap.Pins.Add(pin);
            _pinBoothMap[pin] = booth;
        }
    }
    private string GetErrorTitleText()
    {
        return LanguageService.CurrentLanguage switch
        {
            "en" => "Error",
            "zh" => "错误",
            "fr" => "Erreur",
            "ja" => "エラー",
            "ko" => "오류",
            "es" => "Error",
            "it" => "Errore",
            "ru" => "Ошибка",
            _ => "Lỗi"
        };
    }

    private string GetOkText()
    {
        return LanguageService.CurrentLanguage switch
        {
            "zh" => "确定",
            "fr" => "OK",
            "ja" => "OK",
            "ko" => "확인",
            "es" => "Aceptar",
            "it" => "OK",
            "ru" => "ОК",
            _ => "OK"
        };
    }
    private async void OnGpsModeChanged(object? sender, bool enabled)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await ApplyGpsModeAsync(enabled);
        });
    }
    private async void OnAutoBoothStateChanged(object? sender, AutoBoothStateChangedEventArgs e)
    {
        try
        {
            _currentNearestBooth = e.NearestBooth;

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (e.CurrentLocation != null)
                {
                    LocationLabel.Text =
     $"{LanguageService.T("Ui_CurrentLocation")}: {e.CurrentLocation.Latitude:F6}, {e.CurrentLocation.Longitude:F6}";


                    FoodMap.IsShowingUser = true;

                    var newLoc = new Location(e.CurrentLocation.Latitude, e.CurrentLocation.Longitude);

                    bool shouldMoveMap = false;

                    if (_lastMapCenterLocation == null)
                    {
                        shouldMoveMap = true;
                    }
                    else
                    {
                        var movedDistance = Location.CalculateDistance(
                            _lastMapCenterLocation,
                            newLoc,
                            DistanceUnits.Kilometers) * 1000.0;

                        if (movedDistance > 15 || DateTime.UtcNow - _lastMapMoveUtc > TimeSpan.FromSeconds(10))

                        {
                            shouldMoveMap = true;
                        }
                    }

                    if (shouldMoveMap)
                    {
                        FoodMap.MoveToRegion(MapSpan.FromCenterAndRadius(newLoc, Distance.FromMeters(120)));
                        _lastMapCenterLocation = newLoc;
                        _lastMapMoveUtc = DateTime.UtcNow;
                    }
                }


                BoothCountLabel.Text = $"{LanguageService.T("Ui_BoothCount")}: {_booths.Count}";


                if (e.NearestBooth != null)
                {

                    NearestBoothName.Text = LocalizeBoothName(e.NearestBooth);
                    NearestBoothDistance.Text = $"{LanguageService.T("Ui_Distance")}: {e.NearestDistanceMeters:0} m";

                    OpenNearestButton.IsEnabled = true;

                    if (NearestBoothLabel != null)
                        NearestBoothLabel.Text = $"{LanguageService.T("Ui_NearestBoothHeader")}: {LocalizeBoothName(e.NearestBooth)} ({e.NearestDistanceMeters:0}m)";

                    if (!string.IsNullOrWhiteSpace(e.ActiveBoothId) &&
                        e.ActiveBoothId == e.NearestBooth.Id)
                    {
                        GpsStatusLabel.Text = $"{LanguageService.T("Ui_EnteredZone")}: {LocalizeBoothName(e.NearestBooth)}";
                        GpsStatusLabel.TextColor = Color.Parse("#6D5DF6");
                    }
                    else
                    {
                        GpsStatusLabel.Text = $"{LanguageService.T("Ui_GpsChecking")}";
                        GpsStatusLabel.TextColor = Colors.White;
                    }
                }
                else
                {
                    NearestBoothName.Text = LanguageService.T("Ui_NearestUnknown");
                    NearestBoothDistance.Text = "";
                    OpenNearestButton.IsEnabled = false;

                    if (NearestBoothLabel != null)
                        NearestBoothLabel.Text = LanguageService.T("Ui_NearestBoothNotFound");

                    GpsStatusLabel.Text = LanguageService.T("Ui_GpsChecking");
                    GpsStatusLabel.TextColor = Colors.White;
                }
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Lỗi StateChanged: {ex}");
        }
    }

    private async void OnGpsModeClicked(object sender, EventArgs e)
    {
        try
        {
            if (!_gpsModeEnabled)
            {
                // đảm bảo GPS tracking đang bật
                var ok = await _autoBoothNavigatorService.StartAsync();
                if (!ok)
                {
                    await DisplayAlertAsync(
    LanguageService.T("Ui_GpsMode"),
    LanguageService.T("Ui_GpsNotEnabledOrPermissionDenied"),
    GetOkText());
                    return;
                }

                _autoBoothNavigatorService.SetAutoNarrationEnabled(true);

                _gpsModeEnabled = true;
                GpsStatusLabel.Text = $"{LanguageService.T("Ui_GpsMode")}: {LanguageService.T("Ui_GpsAutoEnabled")}";
                GpsStatusLabel.TextColor = Color.Parse("#6D5DF6");

                await DisplayAlertAsync(
    LanguageService.T("Ui_GpsMode"),
    LanguageService.T("Ui_GpsAutoEnabledMessage"),
    GetOkText());
            }
            else
            {
                // chỉ tắt auto narration, KHÔNG tắt tracking
                _autoBoothNavigatorService.SetAutoNarrationEnabled(false);

                _gpsModeEnabled = false;
                GpsStatusLabel.Text = $"{LanguageService.T("Ui_GpsMode")}: {LanguageService.T("Ui_GpsManualEnabled")}";
                GpsStatusLabel.TextColor = Colors.White;

                // Giữ vị trí user trên bản đồ
                FoodMap.IsShowingUser = true;

                await DisplayAlertAsync(
    LanguageService.T("Ui_GpsMode"),
    LanguageService.T("Ui_GpsManualEnabledMessage"),
    GetOkText());
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
    LanguageService.T("Ui_GpsModeError"),
    ex.ToString(),
    GetOkText());
        }
    }

    private async void OnPinMarkerClicked(object? sender, PinClickedEventArgs e)
    {
        e.HideInfoWindow = true;
        if (sender is Pin pin && _pinBoothMap.TryGetValue(pin, out var booth))
        {
            await Shell.Current.GoToAsync($"{nameof(BoothDetailPage)}?boothId={booth.Id}&trigger=MapTap");
        }
    }

    private async void OnOpenNearestClicked(object sender, EventArgs e)
    {
        if (_currentNearestBooth != null)
        {
            await Shell.Current.GoToAsync($"{nameof(BoothDetailPage)}?boothId={_currentNearestBooth.Id}&trigger=NearestButton");
        }
    }

    private async void OnRefreshLocationClicked(object sender, EventArgs e)
    {
        await _autoBoothNavigatorService.ForceRefreshAsync();
    }
    private string LocalizeBoothName(Booth booth)
    {
        return LanguageService.IsVi
            ? booth.NameVi
            : (!string.IsNullOrWhiteSpace(booth.NameEn) ? booth.NameEn : booth.NameVi);
    }
    private async Task ApplyGpsModeAsync(bool enabled)
    {
        _gpsModeEnabled = enabled;

        if (enabled)
        {
            var ok = await _autoBoothNavigatorService.StartAsync();
            if (!ok)
            {
                GpsStatusLabel.Text = LanguageService.T("Ui_GpsNotEnabledOrPermissionDenied");
                FoodMap.IsShowingUser = false;
                return;
            }

            _autoBoothNavigatorService.SetAutoNarrationEnabled(true);
            FoodMap.IsShowingUser = true;
            GpsStatusLabel.Text = GetGpsOnText();
            await _autoBoothNavigatorService.ForceRefreshAsync();
        }
        else
        {
            _autoBoothNavigatorService.SetAutoNarrationEnabled(false);
            _autoBoothNavigatorService.Stop();

            FoodMap.IsShowingUser = false;

            LocationLabel.Text = LanguageService.T("Ui_Empty");
            NearestBoothName.Text = LanguageService.T("Ui_NearestUnknown");
            NearestBoothLabel.Text = LanguageService.T("Ui_NearestBoothNotFound");
            NearestBoothDistance.Text = LanguageService.T("Ui_Empty");
            OpenNearestButton.IsEnabled = false;

            GpsStatusLabel.Text = GetGpsOffText();
            GpsStatusLabel.TextColor = Colors.White;
        }
    }
    private string GetGpsOnText()
    {
        return LanguageService.CurrentLanguage switch
        {
            "en" => "GPS is on",
            "fr" => "Le GPS est activé",
            "zh" => "GPS 已开启",
            _ => "GPS đang bật"
        };
    }

    private string GetGpsOffText()
    {
        return LanguageService.CurrentLanguage switch
        {
            "en" => "GPS is off",
            "fr" => "Le GPS est désactivé",
            "zh" => "GPS 已关闭",
            _ => "GPS đang tắt"
        };
    }

}
