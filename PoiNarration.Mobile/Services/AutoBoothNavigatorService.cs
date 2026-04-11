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

    // TÁCH GPS TRACKING và AUTO NARRATION
    private bool _autoNarrationEnabled = true;

    // Chặn trigger lặp quá nhanh cùng 1 booth
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
            // Đã bật rồi thì ép lấy điểm GPS mới nhất ngay
            await _locationTrackingService.StartListeningAsync();
            return true;
        }

        var ok = await _locationTrackingService.StartAsync();
        if (!ok)
            return false;

        _locationTrackingService.LocationChanged -= OnLocationChanged;
        _locationTrackingService.LocationChanged += OnLocationChanged;

        _isStarted = true;

        // ép lấy ngay điểm đầu tiên để UI map / nearest booth update
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

            // 1) Luôn update UI nearest booth
            StateChanged?.Invoke(this, new AutoBoothStateChangedEventArgs
            {
                CurrentLocation = loc,
                NearestBooth = result.NearestBooth,
                NearestDistanceMeters = result.NearestDistanceMeters,
                ActiveBoothId = result.ActiveBoothId
            });

            // 2) Nếu chỉ muốn tracking mà không auto narration thì dừng ở đây
            if (!_autoNarrationEnabled)
                return;

            // 3) Chưa đủ điều kiện trigger
            if (!result.ShouldTrigger || result.TriggeredBooth == null)
                return;

            var booth = result.TriggeredBooth;

            // 4) Chặn trigger lặp quá nhanh cùng 1 booth
            if (_lastTriggeredBoothId == booth.Id &&
                DateTime.UtcNow - _lastTriggeredUtc < _triggerGuard)
            {
                return;
            }

            _lastTriggeredBoothId = booth.Id;
            _lastTriggeredUtc = DateTime.UtcNow;

            // 5) Báo cho BoothDetailPage đổi booth
            BoothTriggered?.Invoke(this, booth);

            // 6) Nếu đang điều hướng rồi thì không chồng thêm
            if (_isNavigating)
                return;

            _isNavigating = true;

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                var currentPage = Shell.Current?.CurrentPage;

                // Nếu đang ở detail thì không push page mới nữa
                if (currentPage is not Views.BoothDetailPage)
                {
                    await Shell.Current.GoToAsync($"{nameof(Views.BoothDetailPage)}?boothId={booth.Id}");
                }
            });

            // chờ page dựng xong
            await Task.Delay(300);

            await _narrationService.SpeakBoothAsync(booth, "GPS", loc);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"AutoBoothNavigatorService.OnLocationChanged lỗi: {ex}");
        }
        finally
        {
            _isNavigating = false;
            _gpsLock.Release();
        }
    }
}

public class AutoBoothStateChangedEventArgs : EventArgs
{
    public Location? CurrentLocation { get; set; }
    public Booth? NearestBooth { get; set; }
    public double NearestDistanceMeters { get; set; }
    public string? ActiveBoothId { get; set; }
}
