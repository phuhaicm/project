namespace PoiNarration.Mobile.Services;

public class VisitorSessionService
{
    private const string VisitorIdKey = "visitor_id";
    private const string VisitorCodeKey = "visitor_code";
    private const string DeviceKeyKey = "device_key";
    private const string SessionIdKey = "session_id";

    public void EnsureInitialized()
    {
        var visitorId = Preferences.Get(VisitorIdKey, "");
        if (string.IsNullOrWhiteSpace(visitorId))
        {
            var newVisitorId = Guid.NewGuid().ToString();
            var visitorCode = $"VIS-{Guid.NewGuid():N}".Substring(0, 10).ToUpper();
            var deviceKey = $"device-{Guid.NewGuid():N}";
            var sessionId = Guid.NewGuid().ToString();

            Preferences.Set(VisitorIdKey, newVisitorId);
            Preferences.Set(VisitorCodeKey, visitorCode);
            Preferences.Set(DeviceKeyKey, deviceKey);
            Preferences.Set(SessionIdKey, sessionId);
        }
    }

    public string GetVisitorId()
    {
        EnsureInitialized();
        return Preferences.Get(VisitorIdKey, "");
    }

    public string GetVisitorCode()
    {
        EnsureInitialized();
        return Preferences.Get(VisitorCodeKey, "");
    }

    public string GetDeviceKey()
    {
        EnsureInitialized();
        return Preferences.Get(DeviceKeyKey, "");
    }

    public string GetSessionId()
    {
        EnsureInitialized();
        return Preferences.Get(SessionIdKey, "");
    }

    public void RefreshSession()
    {
        Preferences.Set(SessionIdKey, Guid.NewGuid().ToString());
    }
}
