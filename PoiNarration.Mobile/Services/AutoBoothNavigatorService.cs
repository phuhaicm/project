using Microsoft.Maui.Devices.Sensors;
using PoiNarration.Core.Models;
using System.Threading;

namespace PoiNarration.Mobile.Services;

public class AutoBoothNavigatorService
{
    private readonly LocationTrackingService _locationTrackingService;
    private readonly GeofenceService _geofenceService;
    private readonly NarrationService _narrationService;

    private readonly SemaphoreSlim _gpsLock = new(1, 1);

    private int _gpsSequence;
    private bool _isStarted;
    private bool _isNavigating;

    public Booth? CurrentNearestBooth { get; private set; }
    public double CurrentNearestDistanceMeters { get; private set; } = double.MaxValue;
    public Location? CurrentLocation { get; private set; }

    public event EventHandler<AutoBoothStateChangedEventArgs>? StateChanged;
    public event EventHandler<Booth>? BoothTriggered;

    public AutoBoothNavigatorService(
        LocationTrackingService locationTrackingService,
        GeofenceService geofenceService,
        NarrationService narrationService)
    {
        _locationTrackingService = locationTrackingService;
        _geofenceService = geofenceService;
        _narrationService = narrationService;
    }

    public async Task<bool> StartAsync()
    {
        if (_isStarted)
            return true;

        var ok = await _locationTrackingService.StartAsync();
        if (!ok)
            return false;

        _locationTrackingService.LocationChanged -= OnLocationChanged;
        _locationTrackingService.LocationChanged += OnLocationChanged;

        _isStarted = true;
        return true;
    }

    public void Stop()
    {
        if (!_isStarted)
            return;

        _locationTrackingService.LocationChanged -= OnLocationChanged;
        _locationTrackingService.Stop();
        _geofenceService.Reset();

        _isStarted = false;
        _isNavigating = false;
        CurrentNearestBooth = null;
        CurrentNearestDistanceMeters = double.MaxValue;
        CurrentLocation = null;
    }

    private async void OnLocationChanged(object? sender, Location loc)
    {
        var seq = Interlocked.Increment(ref _gpsSequence);

        await _gpsLock.WaitAsync();
        try
        {
            // Chỉ xử lý điểm GPS mới nhất
            if (seq != _gpsSequence)
                return;

            CurrentLocation = loc;

            var result = await _geofenceService.EvaluateAsync(loc.Latitude, loc.Longitude);

            CurrentNearestBooth = result.NearestBooth;
            CurrentNearestDistanceMeters = result.NearestDistanceMeters;

            StateChanged?.Invoke(this, new AutoBoothStateChangedEventArgs
            {
                CurrentLocation = loc,
                NearestBooth = result.NearestBooth,
                NearestDistanceMeters = result.NearestDistanceMeters,
                ActiveBoothId = result.ActiveBoothId
            });

            if (!result.ShouldTrigger || result.TriggeredBooth == null)
                return;

            var booth = result.TriggeredBooth;

            // báo cho panel/detail biết booth mới
            BoothTriggered?.Invoke(this, booth);

            if (_isNavigating)
                return;

            _isNavigating = true;

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                var currentPage = Shell.Current?.CurrentPage;

                if (currentPage is Views.BoothDetailPage)
                {
                    // BoothDetailPage sẽ tự đổi booth qua event
                }
                else
                {
                    await Shell.Current.GoToAsync($"{nameof(Views.BoothDetailPage)}?boothId={booth.Id}");
                }
            });

            await Task.Delay(400);

            await _narrationService.SpeakBoothAsync(booth, "GPS");
        }
        catch (Exception ex)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                var page = GetCurrentPage();
                if (page != null)
                {
                    await page.DisplayAlertAsync(
                        "Lỗi GPS Auto Booth",
                        ex.ToString(),
                        "OK");
                }
            });
        }
        finally
        {
            _isNavigating = false;
            _gpsLock.Release();
        }
    }

    private static Page? GetCurrentPage()
    {
        var app = Application.Current;
        if (app == null)
            return null;

        if (app.Windows.Count > 0)
            return app.Windows[0].Page;

        return null;
    }
}

public class AutoBoothStateChangedEventArgs : EventArgs
{
    public Location? CurrentLocation { get; set; }
    public Booth? NearestBooth { get; set; }
    public double NearestDistanceMeters { get; set; }
    public string? ActiveBoothId { get; set; }
}
