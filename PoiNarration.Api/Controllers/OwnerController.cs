using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoiNarration.Api.Data;

namespace PoiNarration.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OwnerController : ControllerBase
{
    private readonly AppDbContext _db;

    public OwnerController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("{ownerUserId}/booths")]
    public async Task<IActionResult> GetBoothsByOwner(string ownerUserId)
    {
        var booths = await _db.Booths
            .Where(x => x.OwnerUserId == ownerUserId)
            .ToListAsync();

        return Ok(booths);
    }

    [HttpGet("{ownerUserId}/booths/{boothId}/menu")]
    public async Task<IActionResult> GetMenuByOwner(string ownerUserId, string boothId)
    {
        var booth = await _db.Booths
            .FirstOrDefaultAsync(x => x.Id == boothId && x.OwnerUserId == ownerUserId);

        if (booth == null)
            return Forbid();

        var menu = await _db.BoothMenuItems
            .Where(x => x.BoothId == boothId && !x.IsDeleted)
            .ToListAsync();

        return Ok(menu);
    }
}
