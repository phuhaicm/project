using Microsoft.AspNetCore.Mvc;
using PoiNarration.Api.Data; // Thay bằng namespace DbContext của bạn
using PoiNarration.Api.Models.Entities;

public class UserController : Controller
{
    private readonly AppDbContext _db;

    public UserController(AppDbContext db) => _db = db;

    // 1. Trang danh sách người dùng
    public IActionResult Index()
    {
        var users = _db.AppUsers.ToList();
        return View(users);
    }

    // 2. Trang tạo mới (Giao diện)
    public IActionResult Create() => View();

    // 3. Xử lý lưu (Hành động)
    [HttpPost]
    public async Task<IActionResult> Create(PoiNarration.Web.Models.UserCreateViewModel model)
    {
        if (_db.AppUsers.Any(u => u.Username == model.Username))
        {
            ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại!");
            return View(model);
        }

        var newUser = new AppUser
        {
            Id = Guid.NewGuid().ToString(),
            Username = model.Username,
            FullName = model.FullName,
            Password = model.Password, // Lưu ý: thực tế nên dùng PasswordHash
            PasswordHash = model.Password,
            Role = model.Role
        };

        _db.AppUsers.Add(newUser);
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}