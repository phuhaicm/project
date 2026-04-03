using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoiNarration.Api.Data;
using PoiNarration.Api.DTOs.Auth;

namespace PoiNarration.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;

    public AuthController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var user = await _db.AppUsers
            .FirstOrDefaultAsync(x => x.Username == request.Username);

        if (user == null)
            return Unauthorized("Sai tài khoản.");

        if (user.PasswordHash != request.Password)
            return Unauthorized("Sai mật khẩu.");

        return Ok(new LoginResponse
        {
            UserId = user.Id,
            Username = user.Username,
            Role = user.Role
        });
    }
}