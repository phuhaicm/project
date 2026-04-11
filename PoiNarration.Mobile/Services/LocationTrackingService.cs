using Microsoft.Maui.Devices.Sensors;

namespace PoiNarration.Mobile.Services;

public class LocationTrackingService
{
    // Sự kiện để AutoBoothNavigatorService / Page đăng ký nhận tọa độ
    public event EventHandler<Location>? LocationChanged;

    private bool _isStarted;

    public async Task<bool> StartAsync()
    {
        if (_isStarted)
            return true;

        try
        {
            var ok = await EnsurePermissionAsync();
            if (!ok)
                return false;

            var request = new GeolocationListeningRequest(
                GeolocationAccuracy.Best,
                TimeSpan.FromSeconds(2));

            Geolocation.Default.LocationChanged -= OnLocationChanged;
            Geolocation.Default.LocationChanged += OnLocationChanged;

            await Geolocation.Default.StartListeningForegroundAsync(request);

            _isStarted = true;

            // Lấy luôn vị trí hiện tại để UI không bị "Đang xác định..."
            await StartListeningAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public void Stop()
    {
        if (!_isStarted)
            return;

        Geolocation.Default.LocationChanged -= OnLocationChanged;
        Geolocation.Default.StopListeningForeground();
        _isStarted = false;
    }

    /// <summary>
    /// Ép lấy vị trí ngay lập tức và phát event ra ngoài.
    /// </summary>
    public async Task StartListeningAsync()
    {
        try
        {
            var location = await Geolocation.Default.GetLocationAsync(
                new GeolocationRequest(GeolocationAccuracy.Best));

            if (location != null)
            {
                LocationChanged?.Invoke(this, location);
            }
        }
        catch
        {
            // ignore
        }
    }

    private void OnLocationChanged(object? sender, GeolocationLocationChangedEventArgs e)
    {
        if (e.Location != null)
        {
            LocationChanged?.Invoke(this, e.Location);
        }
    }

    private static async Task<bool> EnsurePermissionAsync()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        }

        return status == PermissionStatus.Granted;
    }
}
