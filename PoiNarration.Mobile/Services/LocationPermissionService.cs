using Microsoft.Maui.ApplicationModel;

namespace PoiNarration.Mobile.Services;

public static class LocationPermissionService
{
    public static async Task<bool> EnsureAsync()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
        if (status != PermissionStatus.Granted)
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

        return status == PermissionStatus.Granted;
    }
}