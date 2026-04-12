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
    public IActionResult Create()
    {
        var guard = EnsureAdmin();
        if (guard != null) return guard;

        var vm = new BoothCreateVm();
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BoothCreateVm model)
    {
        var guard = EnsureAdmin();
        if (guard != null) return guard;

        if (!ModelState.IsValid)
            return View(model);

        string imageUrl = model.ExistingImageUrl ?? "";

        // Upload ảnh nếu có
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

        // Chỉ gửi dữ liệu tiếng Việt gốc.
        // API sẽ tự động sinh translation thông qua ITranslationService.
        var payload = new
        {
            zoneId = model.ZoneId,
            nameVi = model.NameVi,
            descVi = model.DescVi,
            lat = model.Lat,
            lng = model.Lng,
            radiusMeters = model.RadiusMeters,
            priority = model.Priority,
            imageUrl = imageUrl,
            mapUrl = model.MapUrl,
            ttsScriptVi = model.TtsScriptVi,
            audioUrlVi = model.AudioUrlVi,
            isActive = model.IsActive
        };

        var response = await _http.PostAsJsonAsync("api/booths", payload);

        if (!response.IsSuccessStatusCode)
        {
            var errorText = await response.Content.ReadAsStringAsync();
            TempData["Error"] = $"Tạo booth thất bại: {errorText}";
            return View(model);
        }

        TempData["Success"] = "Tạo booth thành công.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var guard = EnsureAdmin();
        if (guard != null) return guard;
        if (string.IsNullOrWhiteSpace(id))
        {
            TempData["Error"] = "Thiếu mã booth cần sửa.";
            return RedirectToAction(nameof(Index));
        }


        var booth = await _http.GetFromJsonAsync<BoothDto>($"api/booths/{id}");
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
            DescVi = booth.DescVi,
            Lat = booth.Lat,
            Lng = booth.Lng,
            RadiusMeters = booth.RadiusMeters,
            Priority = booth.Priority,
            ExistingImageUrl = booth.ImageUrl,
            MapUrl = booth.MapUrl,
            TtsScriptVi = booth.TtsScriptVi,
            AudioUrlVi = booth.AudioUrlVi,
            IsActive = booth.IsActive
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(BoothCreateVm model)
    {
        var guard = EnsureAdmin();
        if (guard != null) return guard;

        if (!ModelState.IsValid)
            return View(model);

        if (string.IsNullOrWhiteSpace(model.Id))
        {
            TempData["Error"] = "Thiếu mã booth cần cập nhật.";
            return View(model);
        }

        string imageUrl = model.ExistingImageUrl ?? "";

        // Upload ảnh mới nếu có
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

        // Chỉ gửi dữ liệu tiếng Việt gốc.
        // API sẽ tự build lại translations sau khi update.
        var payload = new
        {
            zoneId = model.ZoneId,
            nameVi = model.NameVi,
            descVi = model.DescVi,
            lat = model.Lat,
            lng = model.Lng,
            radiusMeters = model.RadiusMeters,
            priority = model.Priority,
            imageUrl = imageUrl,
            mapUrl = model.MapUrl,
            ttsScriptVi = model.TtsScriptVi,
            audioUrlVi = model.AudioUrlVi,
            isActive = model.IsActive
        };

        var response = await _http.PutAsJsonAsync($"api/booths/{model.Id}", payload);

        if (!response.IsSuccessStatusCode)
        {
            var errorText = await response.Content.ReadAsStringAsync();
            TempData["Error"] = $"Cập nhật booth thất bại: {errorText}";
            return View(model);
        }

        TempData["Success"] = "Cập nhật booth thành công.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleActive(string id) // Đã đổi tên hàm cho khớp Giao diện
    {
        var guard = EnsureAdmin();
        if (guard != null) return guard;

        if (string.IsNullOrWhiteSpace(id))
        {
            TempData["Error"] = "Thiếu mã booth cần thao tác.";
            return RedirectToAction(nameof(Index));
        }

        // 1. Lấy thông tin booth hiện tại
        var booth = await _http.GetFromJsonAsync<BoothDto>($"api/booths/{id}");
        if (booth == null)
        {
            TempData["Error"] = "Không tìm thấy booth.";
            return RedirectToAction(nameof(Index));
        }

        // 2. Tạo payload mới, CHỈ ĐẢO NGƯỢC trạng thái IsActive
        var payload = new
        {
            zoneId = booth.ZoneId,
            nameVi = booth.NameVi,
            descVi = booth.DescVi,
            lat = booth.Lat,
            lng = booth.Lng,
            radiusMeters = booth.RadiusMeters,
            priority = booth.Priority,
            imageUrl = booth.ImageUrl,
            mapUrl = booth.MapUrl,
            ttsScriptVi = booth.TtsScriptVi,
            audioUrlVi = booth.AudioUrlVi,

            isActive = !booth.IsActive // <--- ĐIỂM ĂN TIỀN LÀ ĐÂY (Đang true thành false, đang false thành true)
        };

        // 3. Gửi lên API để Update
        var response = await _http.PutAsJsonAsync($"api/booths/{id}", payload);

        if (!response.IsSuccessStatusCode)
        {
            var errorText = await response.Content.ReadAsStringAsync();
            TempData["Error"] = $"Cập nhật trạng thái thất bại: {errorText}";
            return RedirectToAction(nameof(Index));
        }

        // 4. Thông báo thông minh tùy theo trạng thái mới
        TempData["Success"] = booth.IsActive ? "Booth đã được ẨN khỏi ứng dụng." : "Booth đã được MỞ LẠI thành công.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
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
    public async Task<IActionResult> GetOwners()
    {
        var guard = EnsureAdmin();
        if (guard != null) return guard;

        var owners = await _http.GetFromJsonAsync<List<object>>("api/admin/booths/owners")
                     ?? new List<object>();

        return Json(owners);
    }

    private class UploadMediaResponse
    {
        public string Url { get; set; } = "";
    }
}
