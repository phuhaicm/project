using Microsoft.Maui.Devices;

namespace PoiNarration.Mobile.Services
{
    public static class ApiConstants
    {
        private const string MyComputerIp = "192.168.1.237";
        private const string HttpPort = "5151";

        public static string GetBaseUrl()
        {
#if WINDOWS
            return $"http://localhost:{HttpPort}/";
#elif ANDROID
            // Máy ảo Android -> host machine
            if (DeviceInfo.Current.DeviceType == DeviceType.Virtual)
                return $"http://10.0.2.2:{HttpPort}/";

            // Điện thoại thật -> IP LAN của máy tính
            return $"http://{MyComputerIp}:{HttpPort}/";
#else
            return $"http://{MyComputerIp}:{HttpPort}/";
#endif
        }

        public static string BoothsEndpoint => $"{GetBaseUrl()}api/Booths";
    }
}