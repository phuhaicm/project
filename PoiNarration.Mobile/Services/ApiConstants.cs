namespace PoiNarration.Mobile.Services
{
    public static class ApiConstants
    {
        private const string MyComputerIp = "192.168.1.237";
        private const string HttpPort = "5151";

        // ĐỔI Ở ĐÂY:
        private const bool ForceEmulatorMode = false;

        public static string GetBaseUrl()
        {
#if WINDOWS
            return $"http://localhost:{HttpPort}/";
#elif ANDROID
            if (ForceEmulatorMode)
                return $"http://10.0.2.2:{HttpPort}/";

            return $"http://{MyComputerIp}:{HttpPort}/";
#else
            return $"http://{MyComputerIp}:{HttpPort}/";
#endif
        }

        public static string BoothsEndpoint => $"{GetBaseUrl()}api/Booths";
    }
}