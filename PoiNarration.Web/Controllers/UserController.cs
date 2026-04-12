using Microsoft.AspNetCore.Mvc;
using PoiNarration.Web.Models;
using System.Net.Http.Json;

namespace PoiNarration.Web.Controllers;

public class UserController : Controller
{
    private readonly HttpClient _http;

    public UserController(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("Api");
    }

    private string? CurrentRole => HttpContext.Session.GetString("Role");
    private string? CurrentUserId => HttpContext.Session.GetString("UserId");

    private IActionResult? EnsureAdmin()
    {
        if (CurrentRole != "Admin")
            return RedirectToAction("Login", "Account");

        return null;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var guard = EnsureAdmin();
        if (guard != null) return guard;

        try
        {
            var users = await _http.GetFromJsonAsync<List<AppUserDto>>("api/users");
            return View(users ?? new List<AppUserDto>());
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Không tải được danh sách tài khoản: {ex.Message}";
            return View(new List<AppUserDto>());
        }
    }

    [HttpGet]
    public IActionResult Create()
    {
        var guard = EnsureAdmin();
        if (guard != null) return guard;

        return View(new UserCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserCreateViewModel model)
    {
        var guard = EnsureAdmin();
        if (guard != null) return guard;

        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var response = await _http.PostAsJsonAsync("api/users", new
            {
                username = model.Username,
                fullName = model.FullName,
                password = model.Password,
                role = model.Role
            });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                TempData["Error"] = $"Không tạo được tài khoản: {error}";
                return View(model);
            }

            TempData["Success"] = "Tạo tài khoản thành công.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Lỗi tạo tài khoản: {ex.Message}";
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var guard = EnsureAdmin();
        if (guard != null) return guard;

        if (string.IsNullOrWhiteSpace(id))
        {
            TempData["Error"] = "Thiếu mã người dùng cần sửa.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            var user = await _http.GetFromJsonAsync<AppUserDto>($"api/users/{id}");
            if (user == null)
            {
                TempData["Error"] = "Không tìm thấy tài khoản.";
                return RedirectToAction(nameof(Index));
            }

            var vm = new UserCreateViewModel
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Role = user.Role,
                Password = ""
            };

            return View(vm);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Không tải được dữ liệu tài khoản: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UserCreateViewModel model)
    {
        var guard = EnsureAdmin();
        if (guard != null) return guard;

        if (!ModelState.IsValid)
            return View(model);

        if (string.IsNullOrWhiteSpace(model.Id))
        {
            TempData["Error"] = "Thiếu mã người dùng cần cập nhật.";
            return View(model);
        }

        try
        {
            var response = await _http.PutAsJsonAsync($"api/users/{model.Id}", new
            {
                fullName = model.FullName,
                password = string.IsNullOrWhiteSpace(model.Password) ? null : model.Password,
                role = model.Role
            });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                TempData["Error"] = $"Cập nhật tài khoản thất bại: {error}";
                return View(model);
            }

            TempData["Success"] = "Cập nhật tài khoản thành công.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Lỗi cập nhật tài khoản: {ex.Message}";
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var guard = EnsureAdmin();
        if (guard != null) return guard;

        if (string.IsNullOrWhiteSpace(id))
        {
            TempData["Error"] = "Thiếu mã người dùng cần xóa.";
            return RedirectToAction(nameof(Index));
        }

        if (id == CurrentUserId)
        {
            TempData["Error"] = "Bạn không thể tự xóa chính mình.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            var response = await _http.DeleteAsync($"api/users/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                TempData["Error"] = $"Xóa tài khoản thất bại: {error}";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Xóa tài khoản thành công.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Lỗi xóa tài khoản: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }
}
