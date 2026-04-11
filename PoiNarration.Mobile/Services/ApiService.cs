using PoiNarration.Core.Models;
using PoiNarration.Mobile.Models;
using System.Net.Http.Json;
using System.Diagnostics;

namespace PoiNarration.Mobile.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;

        public ApiService()
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri(ApiConstants.GetBaseUrl()),
                Timeout = TimeSpan.FromSeconds(30) // Nên có thêm dòng này: Quá 30s không phản hồi thì tự ngắt
            };
        }

        // 1. HÀM MỚI THÊM: Lấy danh sách tất cả các trạm (Dành cho BoothListPage)
        public async Task<List<BoothCardVm>> GetBoothsAsync()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<BoothCardVm>>("api/Booths");
                return result ?? new List<BoothCardVm>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Lỗi GetBoothsAsync]: {ex.Message}");
                return new List<BoothCardVm>();
            }
        }

        // 2. Hàm cũ của bạn (Đã bọc try...catch)
        public async Task<List<BoothMenuItem>> GetMenuByBoothAsync(string boothId)
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<BoothMenuItem>>($"api/booths/{boothId}/menu");
                return result ?? new List<BoothMenuItem>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Lỗi GetMenuByBoothAsync]: {ex.Message}");
                return new List<BoothMenuItem>();
            }
        }

        // 3. Hàm cũ của bạn (Đã bọc try...catch)
        public async Task<BootstrapSyncResponse?> GetBootstrapAsync()
        {
            try
            {
                return await _http.GetFromJsonAsync<BootstrapSyncResponse>("api/sync/bootstrap");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Lỗi GetBootstrapAsync]: {ex.Message}");
                return null;
            }
        }
  
        // 4. Hàm cũ của bạn (Đã bọc try...catch)
        public async Task PostPlaybackLogAsync(PlaybackLogRequest request)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/playbacklogs", request);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Lỗi PostPlaybackLogAsync]: {ex.Message}");
                // Có thể lưu local database ở đây nếu post thất bại để gửi lại sau
            }
        }
    }
}