namespace PoiNarration.Mobile.Services
{
    public static class ApiConstants
    {
        // IP Wi-Fi máy tính của bạn (Dành cho điện thoại thật)
        private const string MyComputerIp = "192.168.1.237";
        private const string HttpPort = "5151";

        public static string GetBaseUrl()
        {
            // TỰ ĐỘNG NHẬN DIỆN: Nếu đang chạy trên Máy ảo (Emulator)
            if (DeviceInfo.Current.DeviceType == DeviceType.Virtual)
            {
                return $"http://10.0.2.2:{HttpPort}/";
            }

            // Nếu đang cầm Điện thoại thật trên tay
            return $"http://{MyComputerIp}:{HttpPort}/";
        }

        // Không còn sợ dính chữ nữa
        public static string BoothsEndpoint => $"{GetBaseUrl()}api/Booths";
    }
}