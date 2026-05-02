using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoiNarration.Api.Data;
using PoiNarration.Api.Models.Entities;


namespace PoiNarration.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _db;

    public UsersController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<AppUserDto>>> GetAllAsync()
    {
        var users = await _db.AppUsers
            .OrderBy(x => x.Role)
            .ThenBy(x => x.Username)
            .Select(x => new AppUserDto
            {
                Id = x.Id,
                Username = x.Username,
                FullName = x.FullName,
                Role = x.Role
            })
            .ToListAsync();

        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppUserDto>> GetByIdAsync(string id)
    {
        var user = await _db.AppUsers.FirstOrDefaultAsync(x => x.Id == id);
        if (user == null)
            return NotFound();

        return Ok(new AppUserDto
        {
            Id = user.Id,
            Username = user.Username,
            FullName = user.FullName,
            Role = user.Role
        });
    }

    [HttpPost]
    public async Task<ActionResult<AppUserDto>> CreateAsync([FromBody] CreateUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username)
            || string.IsNullOrWhiteSpace(request.Password)
            || string.IsNullOrWhiteSpace(request.FullName)
            || string.IsNullOrWhiteSpace(request.Role))
        {
            return BadRequest("Thiếu thông tin tạo tài khoản.");
        }

        var normalizedUsername = request.Username.Trim();

        var exists = await _db.AppUsers.AnyAsync(x => x.Username == normalizedUsername);
        if (exists)
            return Conflict("Tên đăng nhập đã tồn tại.");

        var user = new AppUser
        {
            Id = Guid.NewGuid().ToString(),
            Username = normalizedUsername,
            Password = request.Password,
            PasswordHash = request.Password,
            FullName = request.FullName.Trim(),
            Role = request.Role.Trim()
        };

        _db.AppUsers.Add(user);
        await _db.SaveChangesAsync();

        var dto = new AppUserDto
        {
            Id = user.Id,
            Username = user.Username,
            FullName = user.FullName,
            Role = user.Role
        };

        return Ok(dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] UpdateUserRequest request)
    {
        var user = await _db.AppUsers.FirstOrDefaultAsync(x => x.Id == id);
        if (user == null)
            return NotFound();

        if (!string.IsNullOrWhiteSpace(request.FullName))
            user.FullName = request.FullName.Trim();

        if (!string.IsNullOrWhiteSpace(request.Role))
            user.Role = request.Role.Trim();

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            user.Password = request.Password;
            user.PasswordHash = request.Password;
        }

        await _db.SaveChangesAsync();
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        var user = await _db.AppUsers.FirstOrDefaultAsync(x => x.Id == id);
        if (user == null)
            return NotFound("Không tìm thấy tài khoản.");

        // Không cho xóa tài khoản admin gốc
        if (user.Username == "admin")
            return BadRequest("Không được xóa tài khoản admin gốc.");

        _db.AppUsers.Remove(user);
        await _db.SaveChangesAsync();

        return NoContent();
    }
    public class AppUserDto
    {
        public string Id { get; set; } = "";
        public string Username { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Role { get; set; } = "";
    }

    public class CreateUserRequest
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Role { get; set; } = "Owner";
    }

    public class UpdateUserRequest
    {
        public string? FullName { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }
}
