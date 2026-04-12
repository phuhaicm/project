using Microsoft.AspNetCore.Mvc;
using PoiNarration.Web.ViewModels;
using System.Net.Http.Json;

namespace PoiNarration.Web.Controllers;

public class AccountController : Controller
{
    private readonly HttpClient _http;

    public AccountController(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("Api");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginVm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVm model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", new
            {
                username = model.Username,
                password = model.Password
            });

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Tài khoản hoặc mật khẩu không đúng.";
                return View(model);
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponseVm>();
            if (result == null)
            {
                ViewBag.Error = "Không đọc được dữ liệu trả về từ API.";
                return View(model);
            }

            HttpContext.Session.SetString("UserId", result.UserId ?? "");
            HttpContext.Session.SetString("Username", result.Username ?? "");
            HttpContext.Session.SetString("Role", result.Role ?? "");

            if (result.Role == "Admin")
                return RedirectToAction("Index", "AdminBooth");

            if (result.Role == "Owner")
                return RedirectToAction("MyBooths", "OwnerMenu");

            return RedirectToAction(nameof(Login));
        }
        catch (Exception ex)
        {
            ViewBag.Error = $"Lỗi hệ thống: {ex.Message}";
            return View(model);
        }
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }

    private class LoginResponseVm
    {
        public string? UserId { get; set; }
        public string? Username { get; set; }
        public string? Role { get; set; }
    }
}
