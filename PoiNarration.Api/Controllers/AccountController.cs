//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using PoiNarration.Api.Data;

//namespace PoiNarration.Api.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//public class AccountController : ControllerBase
//{
//    private readonly AppDbContext _db;

//    public AccountController(AppDbContext db)
//    {
//        _db = db;
//    }

//    // ĐỊNH NGHĨA MODEL NHẬN DỮ LIỆU ĐĂNG NHẬP
//    public class LoginRequest
//    {
//        public string Username { get; set; } = "";
//        public string Password { get; set; } = "";
//    }

//    // API ĐĂNG NHẬP: POST /api/account/login
//    [HttpPost("login")]
//    public async Task<IActionResult> Login([FromBody] LoginRequest request)
//    {
//        if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
//        {
//            return BadRequest("Vui lòng nhập đầy đủ tài khoản và mật khẩu sếp ơi!");
//        }

//        // Tìm User trong Database
//        var user = await _db.AppUsers
//            .FirstOrDefaultAsync(u => u.Username == request.Username && u.Password == request.Password);

//        if (user == null)
//        {
//            return Unauthorized("Tài khoản hoặc mật khẩu không đúng rồi!");
//        }

//        // Trả về thông tin cần thiết để Web lưu lại (localStorage)
//        return Ok(new
//        {
//            user.Id,
//            user.Username,
//            user.FullName,
//            user.Role // "Admin" hoặc "Owner"
//        });
//    }
//}