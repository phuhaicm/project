namespace PoiNarration.Mobile.Services;

public class VisitorSessionService
{
    private const string DeviceKeyKey = "device_key";
    private const string SessionIdKey = "session_id";

    private const string VisitorIdServerKey = "visitor_id_server";
    private const string VisitorCodeKey = "visitor_code";
    private const string VisitorDisplayNameKey = "visitor_display_name";

    public void EnsureInitialized()
    {
        // 1 thiết bị chỉ tạo device_key đúng 1 lần
        var deviceKey = Preferences.Get(DeviceKeyKey, "");
        if (string.IsNullOrWhiteSpace(deviceKey))
        {
            Preferences.Set(DeviceKeyKey, $"device-{Guid.NewGuid():N}");
        }

        // session_id có thể refresh mỗi lần mở app nếu bạn muốn theo dõi session
        var sessionId = Preferences.Get(SessionIdKey, "");
        if (string.IsNullOrWhiteSpace(sessionId))
        {
            Preferences.Set(SessionIdKey, Guid.NewGuid().ToString());
        }
    }

    public void RefreshSession()
    {
        Preferences.Set(SessionIdKey, Guid.NewGuid().ToString());
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

    public string GetVisitorIdServer()
    {
        return Preferences.Get(VisitorIdServerKey, "");
    }

    public string GetVisitorCode()
    {
        return Preferences.Get(VisitorCodeKey, "");
    }

    public string GetVisitorDisplayName()
    {
        return Preferences.Get(VisitorDisplayNameKey, "");
    }

    public void SaveRegisteredVisitor(string visitorId, string visitorCode, string displayName)
    {
        Preferences.Set(VisitorIdServerKey, visitorId ?? "");
        Preferences.Set(VisitorCodeKey, visitorCode ?? "");
        Preferences.Set(VisitorDisplayNameKey, displayName ?? "");
    }

    public void ClearRegisteredVisitor()
    {
        Preferences.Remove(VisitorIdServerKey);
        Preferences.Remove(VisitorCodeKey);
        Preferences.Remove(VisitorDisplayNameKey);
    }
}