
using Microsoft.AspNetCore.Mvc;
using PoiNarration.Web.Models;
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
    public async Task<IActionResult> Login(LoginVm model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var response = await _http.PostAsJsonAsync("api/auth/login", new
        {
            username = model.Username,
            password = model.Password
        });

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Đăng nhập thất bại.";
            return View(model);
        }

        var login = await response.Content.ReadFromJsonAsync<LoginResponse>();
        if (login == null)
        {
            ViewBag.Error = "Không đọc được dữ liệu đăng nhập.";
            return View(model);
        }

        HttpContext.Session.SetString("UserId", login.UserId);
        HttpContext.Session.SetString("Username", login.Username);
        HttpContext.Session.SetString("Role", login.Role);

        if (login.Role == "Admin")
            return RedirectToAction("Index", "AdminBooth");

        return RedirectToAction("MyBooths", "OwnerMenu");
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }
}
