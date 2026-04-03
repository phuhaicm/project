namespace PoiNarration.Mobile.Services;

public static class ApiConstants
{
    public static string GetBaseUrl()
    {
        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            // Android emulator gọi máy host bằng 10.0.2.2
            return "http://10.0.2.2:7115/";
        }

        return "https://localhost:7115/";
    }
}