using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoiNarration.Api.Data;
using PoiNarration.Api.DTOs.Admin;

namespace PoiNarration.Api.Controllers;

[ApiController]
[Route("api/admin/booths")]
public class AdminBoothController : ControllerBase
{
    private readonly AppDbContext _db;

    public AdminBoothController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPut("{boothId}/assign-owner")]
    public async Task<IActionResult> AssignOwner(string boothId, [FromBody] AssignOwnerRequest request)
    {
        var booth = await _db.Booths.FirstOrDefaultAsync(x => x.Id == boothId);
        if (booth == null)
            return NotFound("Không tìm thấy booth.");

        var ownerExists = await _db.AppUsers.AnyAsync(x => x.Id == request.OwnerUserId && x.Role == "Owner");
        if (!ownerExists)
            return BadRequest("Owner không hợp lệ.");

        booth.OwnerUserId = request.OwnerUserId;
        await _db.SaveChangesAsync();

        return Ok(booth);
    }

    [HttpGet("owners")]
    public async Task<IActionResult> GetOwners()
    {
        var owners = await _db.AppUsers
            .Where(x => x.Role == "Owner")
            .Select(x => new
            {
                x.Id,
                x.Username,
                x.Role
            })
            .ToListAsync();

        return Ok(owners);
    }
}
