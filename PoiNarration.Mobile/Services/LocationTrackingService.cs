using Microsoft.Maui.Devices.Sensors;

namespace PoiNarration.Mobile.Services;

public class LocationTrackingService
{
    // Sự kiện để các Page (như MapPage) đăng ký nhận tọa độ
    public event EventHandler<Location>? LocationChanged;

    private bool _isStarted;

    /// <summary>
    /// Khởi tạo việc lắng nghe tọa độ trong Foreground
    /// </summary>
    public async Task<bool> StartAsync()
    {
        // Kiểm tra nếu đã chạy rồi thì không khởi chạy lại để tránh tốn tài nguyên
        if (_isStarted)
            return true;

        try
        {
            // Kiểm tra và yêu cầu quyền truy cập vị trí
            var ok = await EnsurePermissionAsync();
            if (!ok) return false;

            // Cấu hình yêu cầu: 
            // - Độ chính xác tốt nhất (Best)
            // - Cập nhật mỗi 2 giây (nhạy hơn để phù hợp với việc di chuyển bộ)
            var request = new GeolocationListeningRequest(
                GeolocationAccuracy.Best,
                TimeSpan.FromSeconds(2));

            Geolocation.Default.LocationChanged += OnLocationChanged;
            await Geolocation.Default.StartListeningForegroundAsync(request);

            _isStarted = true;
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Dừng việc lắng nghe GPS và giải phóng sự kiện
    /// </summary>
    public void Stop()
    {
        if (!_isStarted)
            return;

        Geolocation.Default.LocationChanged -= OnLocationChanged;
        Geolocation.Default.StopListeningForeground();
        _isStarted = false;
    }

    /// <summary>
    /// Phương thức bổ sung để "đánh thức" hoặc lấy vị trí tức thời ngay lập tức
    /// </summary>
    public async Task StartListeningAsync()
    {
        try
        {
            // Ép hệ thống lấy tọa độ một lần để khởi động chip GPS
            await Geolocation.Default.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium));
        }
        catch { /* Bỏ qua lỗi nếu không lấy được tọa độ tức thời */ }
    }

    private void OnLocationChanged(object? sender, GeolocationLocationChangedEventArgs e)
    {
        if (e.Location != null)
        {
            // Bắn sự kiện tọa độ ra ngoài (ví dụ: MapPage sẽ nhận được)
            LocationChanged?.Invoke(this, e.Location);
        }
    }


    private static async Task<bool> EnsurePermissionAsync()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        }

        return status == PermissionStatus.Granted;
    }
}