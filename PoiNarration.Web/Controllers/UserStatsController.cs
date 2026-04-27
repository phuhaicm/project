using Microsoft.AspNetCore.Mvc;
using PoiNarration.Web.Models;
using System.Net.Http.Json;

namespace PoiNarration.Web.Controllers;

public class UserStatsController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public UserStatsController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("Api");

        var visitors = await client.GetFromJsonAsync<List<VisitorUserDto>>("api/visitors")
                       ?? new List<VisitorUserDto>();

        var stats = await client.GetFromJsonAsync<VisitorStatsVm>("api/visitors/stats")
                    ?? new VisitorStatsVm();

        var activityDetails = await client.GetFromJsonAsync<List<VisitorActivityDetailDto>>("api/visitors/activity-details")
                             ?? new List<VisitorActivityDetailDto>();

        ViewBag.Stats = stats;
        ViewBag.ActivityDetails = activityDetails;

        return View(visitors);
    }
}
