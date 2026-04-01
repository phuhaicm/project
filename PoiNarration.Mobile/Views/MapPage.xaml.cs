using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using PoiNarration.Core.Models;
using PoiNarration.Core.Utils;
using PoiNarration.Mobile.Services;

namespace PoiNarration.Mobile.Views;

public partial class MapPage : ContentPage
{
    private readonly AppDatabase _db;
    private readonly SeedService _seed;
    private readonly LocationService _locationService;
    private CancellationTokenSource? _trackingCts;
    private readonly NarrationService _narrationService;
    private readonly GeofenceService _geofenceService;
    private bool _isHandlingTrigger = false;
    private List<Booth> _booths = new();

    public MapPage()
    {
        InitializeComponent();

        var services = Application.Current?.Handler?.MauiContext?.Services
                       ?? throw new Exception("Services is null");

        _db = services.GetRequiredService<AppDatabase>();
        _seed = new SeedService(_db);
        _locationService = services.GetRequiredService<LocationService>();
        _narrationService = services.GetRequiredService<NarrationService>();
        _geofenceService = services.GetRequiredService<GeofenceService>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _seed.EnsureSeededAsync();
        await _db.InitAsync();

        _booths = await _db.GetAllBoothsAsync();
        LoadBoothPins();

        await SetupGpsAsync();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _trackingCts?.Cancel();
    }

    private void LoadBoothPins()
    {
        FoodMap.Pins.Clear();

        foreach (var booth in _booths)
        {
            var pin = new Pin
            {
                Label = booth.NameVi,
                Address = booth.DescVi,
                Type = PinType.Place,
                Location = new Location(booth.Lat, booth.Lng)
            };

            FoodMap.Pins.Add(pin);
        }

        BoothCountLabel.Text = $"Số booth: {_booths.Count}, số pin: {FoodMap.Pins.Count}";
    }


    private async Task SetupGpsAsync()
    {
        var granted = await _locationService.EnsurePermissionAsync();

        if (!granted)
        {
            GpsStatusLabel.Text = "GPS: chưa được cấp quyền";
            return;
        }

        GpsStatusLabel.Text = "GPS: đã cấp quyền";

        var location = await _locationService.GetCurrentLocationAsync();
        if (location != null)
        {
            UpdateMapAndNearest(location);
        }

        _trackingCts = new CancellationTokenSource();

        _ = _locationService.StartListeningAsync(async loc =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                UpdateMapAndNearest(loc);
            });

            await CheckGeofenceAndNarrateAsync(loc);

        }, _trackingCts.Token);
    }

    private void UpdateMapAndNearest(Location location)
    {
        LocationLabel.Text = $"Vị trí hiện tại: {location.Latitude:F6}, {location.Longitude:F6}";

        if (_booths.Count == 0)
        {
            NearestBoothLabel.Text = "Gian gần nhất: không có dữ liệu";
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

        // ✅ Focus map vào TRUNG ĐIỂM giữa user và nearest booth
        var centerLat = (location.Latitude + nearest.Booth.Lat) / 2.0;
        var centerLng = (location.Longitude + nearest.Booth.Lng) / 2.0;

        // ✅ Radius lớn hơn khoảng cách nearest để chắc chắn pin nằm trong vùng nhìn thấy
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

        var triggeredBooth = await _geofenceService.CheckAndGetTriggeredBoothAsync(location, _booths);

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
}