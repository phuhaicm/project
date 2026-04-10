using Microsoft.Maui.Devices.Sensors;

namespace PoiNarration.Mobile.Services;

public class LocationTrackingService
{
    public event EventHandler<Location>? LocationChanged;

    public async Task<bool> StartAsync()
    {
        try
        {
            var ok = await EnsurePermissionAsync();
            if (!ok) return false;

            var request = new GeolocationListeningRequest(
                GeolocationAccuracy.Best,
                TimeSpan.FromSeconds(5));

            Geolocation.Default.LocationChanged += OnLocationChanged;
            await Geolocation.Default.StartListeningForegroundAsync(request);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public void Stop()
    {
        Geolocation.Default.LocationChanged -= OnLocationChanged;
        Geolocation.Default.StopListeningForeground();
    }

    private void OnLocationChanged(object? sender, GeolocationLocationChangedEventArgs e)
    {
        if (e.Location != null)
        {
            LocationChanged?.Invoke(this, e.Location);
        }
    }
    public async Task StartListeningAsync()
    {
        // Đây là lệnh kích hoạt GPS của MAUI
        await Geolocation.Default.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium));
    }
    private static async Task<bool> EnsurePermissionAsync()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
        if (status != PermissionStatus.Granted)
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

        return status == PermissionStatus.Granted;
    }
}