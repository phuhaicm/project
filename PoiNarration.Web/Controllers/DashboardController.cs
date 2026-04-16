using Microsoft.AspNetCore.Mvc;
using PoiNarration.Web.Models;
using PoiNarration.Web.ViewModels;
using System.Net.Http.Json;

namespace PoiNarration.Web.Controllers;

public class DashboardController : Controller
{
    private readonly HttpClient _http;

    public DashboardController(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("Api");
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var vm = new DashboardIndexVm();

        vm.Summary = await _http.GetFromJsonAsync<DashboardSummaryDto>("api/dashboard/summary")
                     ?? new DashboardSummaryDto();

        vm.TopBooths = await _http.GetFromJsonAsync<List<DashboardTopBoothDto>>("api/dashboard/top-booths")
                      ?? new List<DashboardTopBoothDto>();

        vm.LatestLogs = await _http.GetFromJsonAsync<List<LatestPlaybackLogDto>>("api/dashboard/latest-logs")
                       ?? new List<LatestPlaybackLogDto>();

        var visitorStats = await _http.GetFromJsonAsync<VisitorStatsVm>("api/visitors/stats")
                          ?? new VisitorStatsVm();

        vm.TopVisitorLanguages = visitorStats.TopLanguages;
        vm.TopVisitedBooths = visitorStats.TopBoothsByVisit;

        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Activity()
    {
        var logs = await _http.GetFromJsonAsync<List<LatestPlaybackLogDto>>("api/dashboard/latest-logs")
                   ?? new List<LatestPlaybackLogDto>();

        return View(logs);
    }
}
