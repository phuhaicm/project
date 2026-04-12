using Microsoft.AspNetCore.Mvc;
using PoiNarration.Web.Models;
using PoiNarration.Web.ViewModels;
using System.Net.Http.Json;

namespace PoiNarration.Web.Controllers;

public class OwnerMenuController : Controller
{
    private readonly HttpClient _http;

    public OwnerMenuController(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("Api");
    }

    private string? CurrentUserId => HttpContext.Session.GetString("UserId");
    private string? CurrentRole => HttpContext.Session.GetString("Role");

    private IActionResult? EnsureOwner()
    {
        if (string.IsNullOrWhiteSpace(CurrentUserId) || CurrentRole != "Owner")
        {
            return RedirectToAction("Login", "Account");
        }

        return null;
    }

    [HttpGet]
    public async Task<IActionResult> MyBooths()
    {
        var guard = EnsureOwner();
        if (guard != null) return guard;

        var booths = await _http.GetFromJsonAsync<List<BoothDto>>($"api/owner/{CurrentUserId}/booths")
                     ?? new List<BoothDto>();

        return View(booths);
    }

    [HttpGet]
    public async Task<IActionResult> Index(string boothId)
    {
        var guard = EnsureOwner();
        if (guard != null) return guard;

        if (string.IsNullOrWhiteSpace(boothId))
            return RedirectToAction(nameof(MyBooths));

        ViewBag.BoothId = boothId;

        var menu = await _http.GetFromJsonAsync<List<BoothMenuItemDto>>(
            $"api/owner/{CurrentUserId}/booths/{boothId}/menu")
            ?? new List<BoothMenuItemDto>();

        return View(menu);
    }

    [HttpGet]
    public IActionResult Create(string boothId)
    {
        var guard = EnsureOwner();
        if (guard != null) return guard;

        return View(new OwnerMenuItemVm { BoothId = boothId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(OwnerMenuItemVm model)
    {
        var guard = EnsureOwner();
        if (guard != null) return guard;

        if (!ModelState.IsValid) return View(model);

        string imageUrl = "";

        if (model.ImageFile != null && model.ImageFile.Length > 0)
        {
            using var content = new MultipartFormDataContent();
            using var stream = model.ImageFile.OpenReadStream();
            content.Add(new StreamContent(stream), "file", model.ImageFile.FileName);

            var uploadRes = await _http.PostAsync("api/media/upload", content);
            uploadRes.EnsureSuccessStatusCode();

            var uploadJson = await uploadRes.Content.ReadFromJsonAsync<UploadMediaResponse>();
            imageUrl = uploadJson?.Url ?? "";
        }

        var payload = new
        {
            name = model.Name,
            description = model.Description,
            price = model.Price,
            imageUrl = imageUrl
        };

        // ROUTE ĐÚNG
        var res = await _http.PostAsJsonAsync($"api/boothmenu/{model.BoothId}", payload);
        res.EnsureSuccessStatusCode();

        return RedirectToAction(nameof(Index), new { boothId = model.BoothId });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string boothId, string menuId)
    {
        var guard = EnsureOwner();
        if (guard != null) return guard;

        var menu = await _http.GetFromJsonAsync<List<BoothMenuItemDto>>(
            $"api/owner/{CurrentUserId}/booths/{boothId}/menu")
            ?? new List<BoothMenuItemDto>();

        var item = menu.FirstOrDefault(x => x.Id == menuId);
        if (item == null)
            return RedirectToAction(nameof(Index), new { boothId });

        return View(new OwnerMenuItemVm
        {
            BoothId = boothId,
            MenuId = item.Id,
            Name = item.Name,
            Description = item.Description,
            Price = item.Price,
            ExistingImageUrl = item.ImageUrl
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(OwnerMenuItemVm model)
    {
        var guard = EnsureOwner();
        if (guard != null) return guard;

        if (!ModelState.IsValid) return View(model);

        string imageUrl = model.ExistingImageUrl;

        if (model.ImageFile != null && model.ImageFile.Length > 0)
        {
            using var content = new MultipartFormDataContent();
            using var stream = model.ImageFile.OpenReadStream();
            content.Add(new StreamContent(stream), "file", model.ImageFile.FileName);

            var uploadRes = await _http.PostAsync("api/media/upload", content);
            uploadRes.EnsureSuccessStatusCode();

            var uploadJson = await uploadRes.Content.ReadFromJsonAsync<UploadMediaResponse>();
            imageUrl = uploadJson?.Url ?? imageUrl;
        }

        var payload = new
        {
            name = model.Name,
            description = model.Description,
            price = model.Price,
            imageUrl = imageUrl
        };

        // ROUTE ĐÚNG
        var res = await _http.PutAsJsonAsync($"api/boothmenu/{model.BoothId}/items/{model.MenuId}", payload);
        res.EnsureSuccessStatusCode();

        return RedirectToAction(nameof(Index), new { boothId = model.BoothId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string boothId, string menuId)
    {
        var guard = EnsureOwner();
        if (guard != null) return guard;

        // ROUTE ĐÚNG
        var res = await _http.DeleteAsync($"api/boothmenu/{boothId}/items/{menuId}");
        res.EnsureSuccessStatusCode();

        return RedirectToAction(nameof(Index), new { boothId });
    }

    private class UploadMediaResponse
    {
        public string Url { get; set; } = "";
    }
}