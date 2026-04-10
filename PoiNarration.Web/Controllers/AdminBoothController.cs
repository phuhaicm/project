using Microsoft.AspNetCore.Mvc;
using PoiNarration.Web.Models;
using PoiNarration.Web.ViewModels;
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
    [HttpGet]
    public async Task<IActionResult> Edit(string boothId)
    {
        var guard = EnsureAdmin();
        if (guard != null) return guard;

        var booth = await _http.GetFromJsonAsync<BoothDto>($"api/booths/{boothId}");
        if (booth == null)
        {
            TempData["Error"] = "Không tìm thấy booth.";
            return RedirectToAction(nameof(Index));
        }

        var vm = new BoothCreateVm
        {
            Id = booth.Id,
            ZoneId = booth.ZoneId,
            NameVi = booth.NameVi,
            NameEn = booth.NameEn,
            DescVi = booth.DescVi,
            DescEn = booth.DescEn,
            Lat = booth.Lat,
            Lng = booth.Lng,
            RadiusMeters = booth.RadiusMeters,
            Priority = booth.Priority,
            ExistingImageUrl = booth.ImageUrl,
            MapUrl = booth.MapUrl,
            TtsScriptVi = booth.TtsScriptVi,
            TtsScriptEn = booth.TtsScriptEn,
            AudioUrlVi = booth.AudioUrlVi,
            AudioUrlEn = booth.AudioUrlEn,
            IsActive = booth.IsActive
        };

        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(BoothCreateVm model)
    {
        var guard = EnsureAdmin();
        if (guard != null) return guard;

        if (!ModelState.IsValid)
            return View(model);

        string imageUrl = model.ExistingImageUrl ?? "";

        if (model.ImageFile != null && model.ImageFile.Length > 0)
        {
            using var content = new MultipartFormDataContent();
            using var stream = model.ImageFile.OpenReadStream();
            content.Add(new StreamContent(stream), "file", model.ImageFile.FileName);

            var uploadRes = await _http.PostAsync("api/media/upload", content);
            if (!uploadRes.IsSuccessStatusCode)
            {
                TempData["Error"] = "Upload ảnh booth thất bại.";
                return View(model);
            }

            var uploadJson = await uploadRes.Content.ReadFromJsonAsync<UploadMediaResponse>();
            imageUrl = uploadJson?.Url ?? "";
        }

        var response = await _http.PutAsJsonAsync($"api/booths/{model.Id}", new
        {
            zoneId = model.ZoneId,
            nameVi = model.NameVi,
            nameEn = model.NameEn,
            descVi = model.DescVi,
            descEn = model.DescEn,
            lat = model.Lat,
            lng = model.Lng,
            radiusMeters = model.RadiusMeters,
            priority = model.Priority,
            imageUrl = imageUrl,
            mapUrl = model.MapUrl,
            ttsScriptVi = model.TtsScriptVi,
            ttsScriptEn = model.TtsScriptEn,
            audioUrlVi = model.AudioUrlVi,
            audioUrlEn = model.AudioUrlEn,
            isActive = model.IsActive
        });

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Cập nhật booth thất bại.";
            return View(model);
        }

        TempData["Success"] = "Cập nhật booth thành công.";
        return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    public async Task<IActionResult> AssignOwner(string boothId, string? ownerUserId)
    {
        var guard = EnsureAdmin();
        if (guard != null) return guard;

        try
        {
            var res = await _http.PutAsJsonAsync($"api/admin/booths/{boothId}/assign-owner", new
            {
                ownerUserId = string.IsNullOrWhiteSpace(ownerUserId) ? null : ownerUserId
            });

            if (!res.IsSuccessStatusCode)
            {
                var errorText = await res.Content.ReadAsStringAsync();
                TempData["Error"] = $"Gán owner thất bại: {errorText}";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Cập nhật owner thành công.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Lỗi hệ thống: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }
    [HttpGet]
    public IActionResult Create()
    {
        var guard = EnsureAdmin();
        if (guard != null) return guard;

        var vm = new BoothCreateVm
        {
            Translations = new List<BoothTranslationVm>
        {
            new() { LanguageCode = "vi" },
            new() { LanguageCode = "en" },
            new() { LanguageCode = "zh" },
            new() { LanguageCode = "ja" },
            new() { LanguageCode = "ko" },
            new() { LanguageCode = "fr" },
            new() { LanguageCode = "es" },
            new() { LanguageCode = "it" },
            new() { LanguageCode = "ru" }
        }
        };

        return View(vm);
    }


    [HttpPost]
    public async Task<IActionResult> Create(BoothCreateVm model)
    {
        var guard = EnsureAdmin();
        if (guard != null) return guard;

        if (!ModelState.IsValid)
            return View(model);

        string imageUrl = model.ExistingImageUrl ?? "";

        // 1. Xử lý Upload Ảnh (Giữ nguyên logic của bạn)
        if (model.ImageFile != null && model.ImageFile.Length > 0)
        {
            using var content = new MultipartFormDataContent();
            using var stream = model.ImageFile.OpenReadStream();
            content.Add(new StreamContent(stream), "file", model.ImageFile.FileName);

            var uploadRes = await _http.PostAsync("api/media/upload", content);
            if (!uploadRes.IsSuccessStatusCode)
            {
                TempData["Error"] = "Upload ảnh booth thất bại.";
                return View(model);
            }

            var uploadJson = await uploadRes.Content.ReadFromJsonAsync<UploadMediaResponse>();
            imageUrl = uploadJson?.Url ?? "";
        }

        // 2. Map phần Translations (Đoạn code bạn muốn thêm)
        // Sử dụng ?. để tránh lỗi nếu model.Translations bị null
        var translations = model.Translations?.Select(x => new
        {
            languageCode = x.LanguageCode,
            name = x.Name,
            description = x.Description,
            ttsScript = x.TtsScript,
            audioUrl = x.AudioUrl
        }).ToList();

        // 3. Gửi dữ liệu tới API bao gồm cả translations
        var response = await _http.PostAsJsonAsync("api/booths", new
        {
            zoneId = model.ZoneId,
            nameVi = model.NameVi,
            nameEn = model.NameEn,
            descVi = model.DescVi,
            descEn = model.DescEn,
            lat = model.Lat,
            lng = model.Lng,
            radiusMeters = model.RadiusMeters,
            priority = model.Priority,
            imageUrl = imageUrl,
            mapUrl = model.MapUrl,
            ttsScriptVi = model.TtsScriptVi,
            ttsScriptEn = model.TtsScriptEn,
            audioUrlVi = model.AudioUrlVi,
            audioUrlEn = model.AudioUrlEn,
            isActive = model.IsActive,
            translations = translations // Thêm vào object gửi đi
        });

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Tạo booth thất bại.";
            return View(model);
        }

        TempData["Success"] = "Tạo booth thành công.";
        return RedirectToAction(nameof(Index));
    }
    private class UploadMediaResponse
    {
        public string Url { get; set; } = "";
    }
    [HttpPost]
    public async Task<IActionResult> Delete(string boothId)
    {
        var guard = EnsureAdmin();
        if (guard != null) return guard;

        var booth = await _http.GetFromJsonAsync<BoothDto>($"api/booths/{boothId}");
        if (booth == null)
        {
            TempData["Error"] = "Không tìm thấy booth.";
            return RedirectToAction(nameof(Index));
        }

        var response = await _http.PutAsJsonAsync($"api/booths/{boothId}", new
        {
            zoneId = booth.ZoneId,
            nameVi = booth.NameVi,
            nameEn = booth.NameEn,
            descVi = booth.DescVi,
            descEn = booth.DescEn,
            lat = booth.Lat,
            lng = booth.Lng,
            radiusMeters = booth.RadiusMeters,
            priority = booth.Priority,
            imageUrl = booth.ImageUrl,
            mapUrl = booth.MapUrl,
            ttsScriptVi = booth.TtsScriptVi,
            ttsScriptEn = booth.TtsScriptEn,
            audioUrlVi = booth.AudioUrlVi,
            audioUrlEn = booth.AudioUrlEn,
            isActive = false
        });

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Ẩn booth thất bại.";
            return RedirectToAction(nameof(Index));
        }

        TempData["Success"] = "Booth đã được ẩn.";
        return RedirectToAction(nameof(Index));
    }
}
