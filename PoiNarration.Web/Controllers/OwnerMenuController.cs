using Microsoft.AspNetCore.Mvc;
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

    [HttpGet]
    public IActionResult Create(string boothId)
    {
        return View(new OwnerMenuItemVm { BoothId = boothId });
    }

    [HttpPost]
    public async Task<IActionResult> Create(OwnerMenuItemVm model)
    {
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

        var res = await _http.PostAsJsonAsync($"api/booths/{model.BoothId}/menu", payload);
        res.EnsureSuccessStatusCode();

        TempData["Success"] = "Đã thêm món thành công.";
        return RedirectToAction(nameof(Create), new { boothId = model.BoothId });
    }

    private class UploadMediaResponse
    {
        public string Url { get; set; } = "";
    }
}