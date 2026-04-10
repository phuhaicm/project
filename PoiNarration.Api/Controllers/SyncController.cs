using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoiNarration.Api.Data;

namespace PoiNarration.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SyncController : ControllerBase
{
    private readonly AppDbContext _db;

    public SyncController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("bootstrap")]
    public async Task<IActionResult> Bootstrap()
    {
        var booths = await _db.Booths
            .OrderBy(x => x.Priority)
            .ToListAsync();

        var menuItems = await _db.BoothMenuItems
            .Where(x => !x.IsDeleted)
            .ToListAsync();
        var boothTranslations = await _db.BoothTranslations.ToListAsync();
        var menuTranslations = await _db.BoothMenuItemTranslations.ToListAsync();

        return Ok(new
        {
            booths,
            boothTranslations,
            menuItems,
            menuTranslations
        });
    }
}
