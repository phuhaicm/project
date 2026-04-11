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

    public MapPage(
        AppDatabase db,
        AutoBoothNavigatorService autoBoothNavigatorService,
        NarrationService narrationService,
        ApiService apiService,
        GeofenceService geofenceService)
    {
        InitializeComponent();

        _db = db;
        _autoBoothNavigatorService = autoBoothNavigatorService;
        _geofenceService = geofenceService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            await _db.InitAsync();

            _autoBoothNavigatorService.StateChanged -= OnAutoBoothStateChanged;
            _autoBoothNavigatorService.StateChanged += OnAutoBoothStateChanged;

            await _autoBoothNavigatorService.StartAsync();
            _gpsModeEnabled = true;

            _booths = await _db.GetAllBoothsAsync();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                FoodMap.IsShowingUser = true;   // thêm dòng này
                BoothCountLabel.Text = $"Số booth: {_booths.Count}";
                GpsStatusLabel.Text = "GPS: Đang hoạt động";
                LoadBoothPins();
            });


        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Lỗi MapPage", ex.Message, "OK");
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _autoBoothNavigatorService.StateChanged -= OnAutoBoothStateChanged;
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
                        $"Vị trí hiện tại: {e.CurrentLocation.Latitude:F6}, {e.CurrentLocation.Longitude:F6}";

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
 

                BoothCountLabel.Text = $"Số booth: {_booths.Count}";

                if (e.NearestBooth != null)
                {
                    NearestBoothName.Text = e.NearestBooth.NameVi;
                    NearestBoothDistance.Text = $"Khoảng cách: {e.NearestDistanceMeters:0} m";
                    OpenNearestButton.IsEnabled = true;

                    if (NearestBoothLabel != null)
                        NearestBoothLabel.Text = $"Gian gần nhất: {e.NearestBooth.NameVi} ({e.NearestDistanceMeters:0}m)";

                    if (!string.IsNullOrWhiteSpace(e.ActiveBoothId) &&
                        e.ActiveBoothId == e.NearestBooth.Id)
                    {
                        GpsStatusLabel.Text = $"Đã vào vùng: {e.NearestBooth.NameVi}";
                        GpsStatusLabel.TextColor = Color.Parse("#6D5DF6");
                    }
                    else
                    {
                        GpsStatusLabel.Text = "GPS: Đang kiểm tra...";
                        GpsStatusLabel.TextColor = Colors.White;
                    }
                }
                else
                {
                    NearestBoothName.Text = "Chưa xác định";
                    NearestBoothDistance.Text = "";
                    OpenNearestButton.IsEnabled = false;

                    if (NearestBoothLabel != null)
                        NearestBoothLabel.Text = "Chưa tìm thấy booth gần nhất";

                    GpsStatusLabel.Text = "GPS: Đang kiểm tra...";
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
                    await DisplayAlertAsync("GPS Mode", "Không bật được GPS hoặc chưa cấp quyền vị trí.", "OK");
                    return;
                }

                _autoBoothNavigatorService.SetAutoNarrationEnabled(true);

                _gpsModeEnabled = true;
                GpsStatusLabel.Text = "GPS: ĐANG BẬT AUTO";
                GpsStatusLabel.TextColor = Color.Parse("#6D5DF6");

                await DisplayAlertAsync("GPS Mode", "Đã bật chế độ tự động thuyết minh khi đến gần gian hàng.", "OK");
            }
            else
            {
                // chỉ tắt auto narration, KHÔNG tắt tracking
                _autoBoothNavigatorService.SetAutoNarrationEnabled(false);

                _gpsModeEnabled = false;
                GpsStatusLabel.Text = "GPS: CHỈ THEO DÕI";
                GpsStatusLabel.TextColor = Colors.White;

                // Giữ vị trí user trên bản đồ
                FoodMap.IsShowingUser = true;

                await DisplayAlertAsync("GPS Mode", "Đã tắt chế độ tự động. Bản đồ vẫn tiếp tục hiển thị vị trí của bạn.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Lỗi GPS Mode", ex.ToString(), "OK");
        }
    }

    private async void OnPinMarkerClicked(object? sender, PinClickedEventArgs e)
    {
        e.HideInfoWindow = true;
        if (sender is Pin pin && _pinBoothMap.TryGetValue(pin, out var booth))
        {
            await Shell.Current.GoToAsync($"{nameof(BoothDetailPage)}?boothId={booth.Id}");
        }
    }

    private async void OnOpenNearestClicked(object sender, EventArgs e)
    {
        if (_currentNearestBooth != null)
        {
            await Shell.Current.GoToAsync($"{nameof(BoothDetailPage)}?boothId={_currentNearestBooth.Id}");
        }
    }

    private async void OnRefreshLocationClicked(object sender, EventArgs e)
    {
        await _autoBoothNavigatorService.ForceRefreshAsync();
    }
}
