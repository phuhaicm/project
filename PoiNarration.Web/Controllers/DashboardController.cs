using Microsoft.AspNetCore.Mvc;
using PoiNarration.Web.Models;
using PoiNarration.Web.ViewModels;
using System.Net.Http.Json;

namespace PoiNarration.Web.Controllers;

public class DashboardController : Controller
{
    private readonly HttpClient _http;
    private readonly IConfiguration _configuration;

    public DashboardController(IHttpClientFactory factory, IConfiguration configuration)
    {
        _http = factory.CreateClient("Api");
        _configuration = configuration;
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

        ViewBag.AppDownloadUrl = Url.Action("Index", "AppDownload");
        var apiBaseUrl = _configuration["Api:BaseUrl"]?.TrimEnd('/');
        ViewBag.AppDownloadQrImageUrl = string.IsNullOrWhiteSpace(apiBaseUrl)
            ? ""
            : $"{apiBaseUrl}/uploads/qrcodes/qr-app-download.png";

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