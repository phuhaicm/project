using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using PoiNarration.Core.Models;
using PoiNarration.Core.Utils;
using PoiNarration.Mobile.Services;
using Location = Microsoft.Maui.Devices.Sensors.Location;

namespace PoiNarration.Mobile.Views;

public partial class MapPage : ContentPage
{
    private readonly AppDatabase _db;
    private readonly SeedService _seed;
    private readonly LocationService _locationService;
    private readonly LocationTrackingService _locationTrackingService;
    private readonly ApiService _apiService;
    private CancellationTokenSource? _trackingCts;
    private readonly NarrationService _narrationService;
    private readonly GeofenceService _geofenceService;
    private bool _isHandlingTrigger = false;
    private List<Booth> _booths = new();
    private readonly Dictionary<Pin, Booth> _pinBoothMap = new();
    private Booth? _nearestBooth;
    private string? _lastTriggeredBoothId; // Thêm biến này để tránh lặp trigger
                                           // Constructor mới: Tiêm thẳng tất cả các ông vào đây
    private void UpdateMapPins()
    {
        // Đảm bảo chạy trên MainThread
        MainThread.BeginInvokeOnMainThread(() =>
        {
            FoodMap.Pins.Clear();
            foreach (var booth in _booths)
            {
                var pin = new Microsoft.Maui.Controls.Maps.Pin
                {
                    Label = booth.NameVi,
                    Location = new Location(booth.Lat, booth.Lng),
                    Type = Microsoft.Maui.Controls.Maps.PinType.Place
                };
                FoodMap.Pins.Add(pin);
            }
        });
    }
    public MapPage(
        LocationTrackingService locationTrackingService,
        ApiService apiService,
        AppDatabase db,
        LocationService locationService,
        NarrationService narrationService,
        GeofenceService geofenceService)
    {
        InitializeComponent();

        _locationTrackingService = locationTrackingService;
        _apiService = apiService;
        _db = db;
        _locationService = locationService;
        _narrationService = narrationService;
        _geofenceService = geofenceService;

        _seed = new SeedService(_db);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _locationTrackingService.LocationChanged += OnGpsLocationChanged;

        // 1. Load dữ liệu từ SQLite
        var booths = await _db.GetAllBoothsAsync();
        // 2. Cập nhật UI phải bọc trong MainThread
        MainThread.BeginInvokeOnMainThread(() =>
        {
            _booths.Clear();
            foreach (var b in booths) _booths.Add(b);

            // Cập nhật số lượng booth lên màn hình
            BoothCountLabel.Text = $"Số booth: {booths.Count}";

            // Vẽ lại các điểm ghim trên bản đồ
            LoadBoothPins();
        });

        // 3. Bật GPS
        await _locationTrackingService.StartListeningAsync();
    }

    // Các hàm UpdateMapAndNearest và LoadBoothPins giữ nguyên logic cũ của bạn là ổn rồi

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _locationTrackingService.LocationChanged -= OnGpsLocationChanged;
        _locationTrackingService.Stop();
        _trackingCts?.Cancel();
    }

    private void LoadBoothPins()
    {
        FoodMap.Pins.Clear();
        _pinBoothMap.Clear();

        // Chỉ hiện booth IsActive = true
        foreach (var booth in _booths.Where(b => b.IsActive))
        {
            var pin = new Pin
            {
                Label = string.IsNullOrWhiteSpace(booth.NameVi) ? "Booth " + booth.Id : booth.NameVi,
                Address = booth.DescVi,
                Type = PinType.Place,
                Location = new Location(booth.Lat, booth.Lng)
            };
            pin.MarkerClicked += OnPinMarkerClicked;
            FoodMap.Pins.Add(pin);
            _pinBoothMap[pin] = booth;
        }
        BoothCountLabel.Text = $"Số booth: {FoodMap.Pins.Count}";
    }


    

    private void UpdateMapAndNearest(Location location)
    {
        LocationLabel.Text = $"Vị trí hiện tại: {location.Latitude:F6}, {location.Longitude:F6}";

        if (_booths.Count == 0)
        {
            NearestBoothLabel.Text = "Gian gần nhất: không có dữ liệu";

            // Code mới tích hợp khi không có dữ liệu
            _nearestBooth = null;
            NearestBoothName.Text = "Chưa xác định";
            NearestBoothDistance.Text = "";
            OpenNearestButton.IsEnabled = false;
            return;
        }

        var nearest = _booths
            .Select(b => new
            {
                Booth = b,
                Distance = PoiNarration.Core.Utils.GeoUtils.DistanceInMeters(
                    location.Latitude, location.Longitude,
                    b.Lat, b.Lng)
            })
            .OrderBy(x => x.Distance)
            .First();

        NearestBoothLabel.Text = $"Gian gần nhất: {nearest.Booth.NameVi} ({nearest.Distance:F0}m)";

        // Tích hợp code mới cập nhật UI cho Booth gần nhất
        _nearestBooth = nearest.Booth;
        NearestBoothName.Text = _nearestBooth.NameVi;
        NearestBoothDistance.Text = $"Khoảng cách: {nearest.Distance:0} m";
        OpenNearestButton.IsEnabled = true;

        // Focus map vào TRUNG ĐIỂM giữa user và nearest booth
        var centerLat = (location.Latitude + nearest.Booth.Lat) / 2.0;
        var centerLng = (location.Longitude + nearest.Booth.Lng) / 2.0;
        var radiusMeters = Math.Max(500, nearest.Distance + 200);

        FoodMap.MoveToRegion(
            MapSpan.FromCenterAndRadius(
                new Location(centerLat, centerLng),
                Distance.FromMeters(radiusMeters))
        );
    }

    private async void OnRefreshLocationClicked(object sender, EventArgs e)
    {
        var location = await _locationService.GetCurrentLocationAsync();
        if (location != null)
        {
            UpdateMapAndNearest(location);
        }
    }
    private async Task CheckGeofenceAndNarrateAsync(Location location)
    {
        if (_isHandlingTrigger) return;
        if (_narrationService == null || _geofenceService == null) return;

        var triggeredBooth =
await _geofenceService.CheckAndGetTriggeredBoothAsync(
    location.Latitude,
    location.Longitude);


        if (triggeredBooth == null) return;

        _isHandlingTrigger = true;

        try
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Shell.Current.GoToAsync($"{nameof(BoothDetailPage)}?boothId={triggeredBooth.Id}");
            });

            await _narrationService.SpeakBoothAsync(triggeredBooth, "GPS", location);
        }
        finally
        {
            _isHandlingTrigger = false;
        }
    }
    private async void OnGpsLocationChanged(object? sender, Microsoft.Maui.Devices.Sensors.Location loc)
    {
        // 1. Cập nhật UI (Phải chạy trên MainThread để không bị crash JavaProxyThrowable)
        MainThread.BeginInvokeOnMainThread(() =>
        {
            UpdateMapAndNearest(loc);
        });

        // 2. Kiểm tra vùng Geofence (Bán kính quanh booth)
        var booth = await _geofenceService.CheckAndGetTriggeredBoothAsync(loc.Latitude, loc.Longitude);

        if (booth == null)
        {
            _lastTriggeredBoothId = null; // Đi ra khỏi vùng thì reset để lần sau vào lại vẫn báo
            return;
        }

        // 3. CHỐNG SPAM: Nếu đang đứng yên ở booth cũ thì không làm gì cả
        if (booth.Id == _lastTriggeredBoothId) return;

        _lastTriggeredBoothId = booth.Id;

        // 4. Đánh dấu đã chơi và phát giọng nói AI
        _geofenceService.MarkPlayed(booth.Id);
        await _narrationService.SpeakBoothAsync(booth, "GPS");

        // 5. Tự động nhảy sang trang chi tiết (Phải chạy trên MainThread)
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await Shell.Current.GoToAsync($"{nameof(BoothDetailPage)}?boothId={booth.Id}");
        });

        // 6. Gửi log về Server (Dùng IP 192.168.88.235)
        _ = _apiService.PostPlaybackLogAsync(new PlaybackLogRequest
        {
            BoothId = booth.Id,
            TriggerType = "GPS",
            Language = LanguageService.IsVi ? "vi" : "en",
            Lat = loc.Latitude,
            Lng = loc.Longitude,
            IsCompleted = true,
            SessionId = Guid.NewGuid().ToString()
        });
    }
    // Hàm xử lý khi nhấn vào 1 Pin trên bản đồ
    private async void OnPinMarkerClicked(object? sender, PinClickedEventArgs e)
    {
        e.HideInfoWindow = true; // Ẩn info window mặc định nếu muốn tự xử lý UI

        if (sender is Pin pin && _pinBoothMap.TryGetValue(pin, out var booth))
        {
            // Điều hướng sang trang chi tiết của Booth được nhấn
            await Shell.Current.GoToAsync($"{nameof(BoothDetailPage)}?boothId={booth.Id}");
        }
    }

    // Hàm xử lý khi nhấn nút "Mở gian hàng gần nhất" (OpenNearestButton)
    private async void OnOpenNearestClicked(object sender, EventArgs e)
    {
        if (_nearestBooth != null)
        {
            await Shell.Current.GoToAsync($"{nameof(BoothDetailPage)}?boothId={_nearestBooth.Id}");
        }
    }

}