using PoiNarration.Core.Models;
using PoiNarration.Mobile.Models;
using System.Diagnostics;
using System.Net.Http.Json;

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
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

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

        public async Task<List<BoothMenuItem>> GetMenuByBoothAsync(string boothId)
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<BoothMenuItem>>($"api/boothmenu/{boothId}");
                return result ?? new List<BoothMenuItem>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Lỗi GetMenuByBoothAsync]: {ex.Message}");
                return new List<BoothMenuItem>();
            }
        }

        public async Task<BootstrapSyncResponse?> GetBootstrapAsync()
        {
            try
            {
                var response = await _http.GetAsync("api/sync/bootstrap");
                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API bootstrap trả lỗi {(int)response.StatusCode} {response.ReasonPhrase}. Body: {body}");
                }

                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[Bootstrap JSON]: {json}");

                var result = System.Text.Json.JsonSerializer.Deserialize<BootstrapSyncResponse>(
                    json,
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result == null)
                    throw new Exception("Deserialize bootstrap JSON bị null.");

                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Lỗi GetBootstrapAsync]: {ex}");
                throw;
            }
        }

        public async Task<VisitorRegisterResponse?> RegisterVisitorAsync(VisitorRegisterRequest request)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/visitors/register", request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<VisitorRegisterResponse>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Lỗi RegisterVisitorAsync]: {ex.Message}");
                return null;
            }
        }

        public async Task PostBoothVisitLogAsync(BoothVisitLog request)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/boothvisitlogs", request);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Lỗi PostBoothVisitLogAsync]: {ex.Message}");
                throw;
            }
        }

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
                throw;
            }
        }

        public string ResolveMediaUrl(string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return "dotnet_bot";

            if (Uri.TryCreate(path, UriKind.Absolute, out var absoluteUri))
                return absoluteUri.ToString();

            return new Uri(_http.BaseAddress!, path.TrimStart('/')).ToString();
        }

        public string BaseUrl => _http.BaseAddress?.ToString() ?? "";
        public async Task UpdateVisitorLanguageAsync(string visitorId, string languageCode)
        {
            try
            {
                var response = await _http.PutAsJsonAsync(
                    $"api/visitors/{visitorId}/language",
                    new UpdateVisitorLanguageRequest
                    {
                        PreferredLanguage = languageCode
                    });

                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Lỗi UpdateVisitorLanguageAsync]: {ex.Message}");
                throw;
            }
        }

        public async Task TouchVisitorAsync(string visitorId)
        {
            try
            {
                var response = await _http.PutAsync($"api/visitors/{visitorId}/touch", null);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Lỗi TouchVisitorAsync]: {ex.Message}");
                throw;
            }
        }

    }
    public class UpdateVisitorLanguageRequest
    {
        public string PreferredLanguage { get; set; } = "vi";
    }
    public class VisitorRegisterRequest
    {
        public string DeviceKey { get; set; } = "";
        public string PreferredLanguage { get; set; } = "vi";
        public string? Platform { get; set; }
        public string? AppVersion { get; set; }
    }

    public class VisitorRegisterResponse
    {
        public string VisitorId { get; set; } = "";
        public string VisitorCode { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string PreferredLanguage { get; set; } = "vi";
    }
}
