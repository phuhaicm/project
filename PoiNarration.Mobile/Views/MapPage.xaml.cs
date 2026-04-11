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
    private readonly NarrationService _narrationService;
    private readonly ApiService _apiService;
    private readonly GeofenceService _geofenceService;

    private readonly Dictionary<Pin, Booth> _pinBoothMap = new();
    private List<Booth> _booths = new();
    private Booth? _currentNearestBooth;

    // Flag để tránh việc nhảy trang liên tục khi đang trong quá trình điều hướng
    private bool _isNavigating = false;
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
        _narrationService = narrationService;
        _apiService = apiService;
        _geofenceService = geofenceService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            await _db.InitAsync();

            // Đăng ký sự kiện từ Service tập trung
            _autoBoothNavigatorService.StateChanged -= OnAutoBoothStateChanged;
            _autoBoothNavigatorService.StateChanged += OnAutoBoothStateChanged;

            // Khởi động GPS mặc định khi vào trang (hoặc đợi bấm nút tùy bạn)
            await _autoBoothNavigatorService.StartAsync();
            _gpsModeEnabled = true;

            _booths = await _db.GetAllBoothsAsync();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                BoothCountLabel.Text = $"Số booth: {_booths.Count}";
                GpsStatusLabel.Text = "GPS: Đang hoạt động";
                LoadBoothPins();
            });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi MapPage", ex.Message, "OK");
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // Hủy đăng ký để tránh rò rỉ bộ nhớ
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

    // --- HÀM QUAN TRỌNG NHẤT: XỬ LÝ KHI TRẠNG THÁI GPS/GEOFENCE THAY ĐỔI ---
    private async void OnAutoBoothStateChanged(object? sender, AutoBoothStateChangedEventArgs e)
    {
        try
        {
            _currentNearestBooth = e.NearestBooth;

            // 1. Cập nhật giao diện (Run on UI Thread)
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                if (e.CurrentLocation != null)
                {
                    LocationLabel.Text = $"Vị trí: {e.CurrentLocation.Latitude:F6}, {e.CurrentLocation.Longitude:F6}";

                    // Di chuyển bản đồ theo người dùng
                    FoodMap.IsShowingUser = true;
                    FoodMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                        new Location(e.CurrentLocation.Latitude, e.CurrentLocation.Longitude),
                        Distance.FromMeters(120)));
                }

                if (e.NearestBooth != null)
                {
                    NearestBoothName.Text = e.NearestBooth.NameVi;
                    NearestBoothDistance.Text = $"Khoảng cách: {e.NearestDistanceMeters:0} m";
                    OpenNearestButton.IsEnabled = true;

                    if (NearestBoothLabel != null)
                        NearestBoothLabel.Text = $"Gian gần nhất: {e.NearestBooth.NameVi} ({e.NearestDistanceMeters:0}m)";
                }

                // 2. XỬ LÝ TỰ ĐỘNG (TRIGGER): Nhảy trang + Thuyết minh + Ghi log
                // Kiểm tra e.ShouldTrigger được tính toán từ GeofenceService bên trong AutoBoothNavigator
                if (e.ShouldTrigger && e.TriggeredBooth != null && !_isNavigating)
                {
                    await HandleAutoTrigger(e.TriggeredBooth, e.CurrentLocation);
                }
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Lỗi StateChanged: {ex}");
        }
    }

    private async Task HandleAutoTrigger(Booth booth, Location? loc)
    {
        _isNavigating = true;
        try
        {
            // Cập nhật trạng thái trên màn hình map trước khi nhảy
            GpsStatusLabel.Text = $"Đang vào: {booth.NameVi}";
            GpsStatusLabel.TextColor = Color.Parse("#6D5DF6");

            // A. Điều hướng sang trang chi tiết
            await Shell.Current.GoToAsync($"{nameof(BoothDetailPage)}?boothId={booth.Id}");

            // B. Đợi trang load rồi phát giọng nói
            await Task.Delay(800);
            await _narrationService.SpeakBoothAsync(booth, "GPS");

            // C. Ghi log lên server
            if (loc != null)
            {
                _ = _apiService.PostPlaybackLogAsync(new PlaybackLogRequest
                {
                    BoothId = booth.Id,
                    TriggerType = "GPS",
                    Lat = loc.Latitude,
                    Lng = loc.Longitude,
                    IsCompleted = true
                });
            }
        }
        finally
        {
            _isNavigating = false;
        }
    }

    // --- CÁC SỰ KIỆN TƯƠNG TÁC NGƯỜI DÙNG ---

    private async void OnGpsModeClicked(object sender, EventArgs e)
    {
        if (!_gpsModeEnabled)
        {
            _geofenceService.Reset();
            var ok = await _autoBoothNavigatorService.StartAsync();
            if (ok)
            {
                _gpsModeEnabled = true;
                GpsStatusLabel.Text = "GPS: ĐANG BẬT";
                await DisplayAlert("GPS Mode", "Đã bật chế độ tự động thuyết minh khi đến gần gian hàng.", "OK");
            }
        }
        else
        {
            _autoBoothNavigatorService.Stop();
            _geofenceService.Reset();
            _gpsModeEnabled = false;
            GpsStatusLabel.Text = "GPS: ĐÃ TẮT";
            await DisplayAlert("GPS Mode", "Đã tắt chế độ tự động.", "OK");
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
        // Force khởi động lại nếu cần
        await _autoBoothNavigatorService.StartAsync();
    }
}