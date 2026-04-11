namespace PoiNarration.Mobile.Services
{
    public static class ApiConstants
    {
        // THAY ĐỔI: Nhập đúng IP máy tính của bạn tại đây
        private const string MyComputerIp = "192.168.1.237";
        private const string HttpPort = "5151";

        public static string GetBaseUrl()

        {
           
            // Khi chạy trên ĐIỆN THOẠI THẬT:
            // Phải dùng HTTP (không có 's') và IP thật của máy tính
            return $"http://{MyComputerIp}:{HttpPort}";
        }

        public static string BoothsEndpoint => $"{GetBaseUrl()}api/Booths";
    }
}