namespace PoiNarration.Mobile.Services;

public class LocationService
{
    public async Task<bool> EnsurePermissionAsync()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        if (status != PermissionStatus.Granted)
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

        return status == PermissionStatus.Granted;
    }

    public async Task<Location?> GetCurrentLocationAsync()
    {
        try
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            var location = await Geolocation.Default.GetLocationAsync(request);

            if (location != null)
                return location;

            return await Geolocation.Default.GetLastKnownLocationAsync();
        }
        catch
        {
            return null;
        }
    }

    public async Task StartListeningAsync(Func<Location, Task> onLocationUpdated, CancellationToken token, int intervalMs = 5000)
    {
        while (!token.IsCancellationRequested)
        {
            var location = await GetCurrentLocationAsync();

            if (location != null)
                await onLocationUpdated(location);

            await Task.Delay(intervalMs, token);
        }
    }
}
