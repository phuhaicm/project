using Microsoft.AspNetCore.Mvc;
using PoiNarration.Web.Models;
using System.Net.Http.Json;

namespace PoiNarration.Web.Controllers;

public class AdminBoothController : Controller
{
    private readonly HttpClient _http;

    public AdminBoothController(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("Api");
    }

    private string? CurrentRole => HttpContext.Session.GetString("Role");

    private IActionResult? EnsureAdmin()
    {
        if (CurrentRole != "Admin")
        {
            return RedirectToAction("Login", "Account");
        }

        return null;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var guard = EnsureAdmin();
        if (guard != null) return guard;

        var owners = await _http.GetFromJsonAsync<List<AppUserDto>>("api/admin/booths/owners")
                     ?? new List<AppUserDto>();

        var booths = await _http.GetFromJsonAsync<List<BoothDto>>("api/booths")
                    ?? new List<BoothDto>();

        ViewBag.Owners = owners;
        return View(booths);
    }

    [HttpPost]
    public async Task<IActionResult> AssignOwner(string boothId, string ownerUserId)
    {
        var guard = EnsureAdmin();
        if (guard != null) return guard;

        var response = await _http.PutAsJsonAsync($"api/admin/booths/{boothId}/assign-owner", new
        {
            ownerUserId = ownerUserId
        });

        response.EnsureSuccessStatusCode();

        return RedirectToAction(nameof(Index));
    }
}
