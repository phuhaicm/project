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

    private bool _autoNarrationEnabled = true;

    private string? _lastTriggeredBoothId;
    private DateTime _lastTriggeredUtc = DateTime.MinValue;
    private readonly TimeSpan _triggerGuard = TimeSpan.FromMilliseconds(800);

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

    public bool IsStarted => _isStarted;
    public bool AutoNarrationEnabled => _autoNarrationEnabled;

    public void SetAutoNarrationEnabled(bool enabled)
    {
        _autoNarrationEnabled = enabled;
    }

    public async Task<bool> StartAsync()
    {
        if (_isStarted)
        {
            await _locationTrackingService.StartListeningAsync();
            return true;
        }

        var ok = await _locationTrackingService.StartAsync();
        if (!ok)
            return false;

        _locationTrackingService.LocationChanged -= OnLocationChanged;
        _locationTrackingService.LocationChanged += OnLocationChanged;

        _isStarted = true;

        await _locationTrackingService.StartListeningAsync();
        return true;
    }

    public async Task ForceRefreshAsync()
    {
        if (_isStarted)
        {
            await _locationTrackingService.StartListeningAsync();
        }
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
        _autoNarrationEnabled = true;

        _lastTriggeredBoothId = null;
        _lastTriggeredUtc = DateTime.MinValue;

        CurrentNearestBooth = null;
        CurrentNearestDistanceMeters = double.MaxValue;
        CurrentLocation = null;
    }

    private async void OnLocationChanged(object? sender, Location loc)
    {
        var seq = Interlocked.Increment(ref _gpsSequence);

        Booth? nearestBooth = null;
        double nearestDistance = double.MaxValue;
        string? activeBoothId = null;

        Booth? triggeredBooth = null;
        bool shouldSpeak = false;

        try
        {
            await _gpsLock.WaitAsync();
            try
            {
                if (seq != _gpsSequence)
                    return;

                CurrentLocation = loc;

                var result = await _geofenceService.EvaluateAsync(loc.Latitude, loc.Longitude);

                CurrentNearestBooth = result.NearestBooth;
                CurrentNearestDistanceMeters = result.NearestDistanceMeters;

                nearestBooth = result.NearestBooth;
                nearestDistance = result.NearestDistanceMeters;
                activeBoothId = result.ActiveBoothId;

                if (!_autoNarrationEnabled)
                    return;

                if (!result.ShouldTrigger || result.TriggeredBooth == null)
                    return;

                var booth = result.TriggeredBooth;

                if (_lastTriggeredBoothId == booth.Id &&
                    DateTime.UtcNow - _lastTriggeredUtc < _triggerGuard)
                {
                    return;
                }

                _lastTriggeredBoothId = booth.Id;
                _lastTriggeredUtc = DateTime.UtcNow;

                triggeredBooth = booth;
                shouldSpeak = true;
            }
            finally
            {
                _gpsLock.Release();
            }

            StateChanged?.Invoke(this, new AutoBoothStateChangedEventArgs
            {
                CurrentLocation = loc,
                NearestBooth = nearestBooth,
                NearestDistanceMeters = nearestDistance,
                ActiveBoothId = activeBoothId
            });

            if (!shouldSpeak || triggeredBooth == null)
                return;

            var finalBooth = triggeredBooth;
            var shell = Shell.Current;

            if (shell == null)
                return;

            BoothTriggered?.Invoke(this, finalBooth);

            var currentPage = shell.CurrentPage;
            var shouldNavigate = currentPage is not Views.BoothDetailPage;

            if (shouldNavigate)
            {
                if (_isNavigating)
                    return;

                _isNavigating = true;
                try
                {
                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        if (Shell.Current != null)
                        {
                            await Shell.Current.GoToAsync($"{nameof(Views.BoothDetailPage)}?boothId={finalBooth.Id}&trigger=GPS");
                        }
                    });

                    await Task.Delay(150);
                }
                finally
                {
                    _isNavigating = false;
                }
            }

            await _narrationService.SpeakBoothAsync(finalBooth, "GPS", loc);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"AutoBoothNavigatorService.OnLocationChanged lỗi: {ex}");
        }
    }
}

// ===============================
// ĐỂ NGOÀI CLASS AutoBoothNavigatorService
// ===============================
public class AutoBoothStateChangedEventArgs : EventArgs
{
    public Location? CurrentLocation { get; set; }
    public Booth? NearestBooth { get; set; }
    public double NearestDistanceMeters { get; set; }
    public string? ActiveBoothId { get; set; }
}