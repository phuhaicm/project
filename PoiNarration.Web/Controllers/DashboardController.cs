using Microsoft.AspNetCore.Mvc;
using PoiNarration.Web.Models;
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
        var topBooths = await _http.GetFromJsonAsync<List<DashboardTopBoothDto>>("api/dashboard/top-booths")
                        ?? new List<DashboardTopBoothDto>();

        var booths = await _http.GetFromJsonAsync<List<BoothDto>>("api/booths")
                    ?? new List<BoothDto>();

        var owners = await _http.GetFromJsonAsync<List<AppUserDto>>("api/admin/booths/owners")
                     ?? new List<AppUserDto>();

        ViewBag.TotalBooths = booths.Count;
        ViewBag.TotalOwners = owners.Count;
        ViewBag.TotalPlayback = topBooths.Sum(x => x.Count);

        return View(topBooths);
    }
}
