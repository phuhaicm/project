using System.Net.Http.Json;
using PoiNarration.Core.Models;

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
}