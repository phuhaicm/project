using PoiNarration.Core.Models;
using PoiNarration.Mobile.Models;
using System.Net.Http.Json;

namespace PoiNarration.Mobile.Services;

public class ApiService
{
    private readonly HttpClient _http;

    public ApiService()
    {
        _http = new HttpClient
        {
            BaseAddress = new Uri(ApiConstants.GetBaseUrl())
        };
    }

    public async Task<List<BoothMenuItem>> GetMenuByBoothAsync(string boothId)
    {
        var result = await _http.GetFromJsonAsync<List<BoothMenuItem>>($"api/booths/{boothId}/menu");
        return result ?? new List<BoothMenuItem>();
    }

    public async Task<BootstrapSyncResponse?> GetBootstrapAsync()
    {
        return await _http.GetFromJsonAsync<BootstrapSyncResponse>("api/sync/bootstrap");
    }

    public async Task PostPlaybackLogAsync(PlaybackLogRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/playbacklogs", request);
        response.EnsureSuccessStatusCode();
    }
    
}