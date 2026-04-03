using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoiNarration.Api.Data;
using PoiNarration.Api.DTOs.Sync;

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
    public async Task<ActionResult<BootstrapSyncResponse>> Bootstrap()
    {
        var booths = await _db.Booths.ToListAsync();
        var menuItems = await _db.BoothMenuItems
            .Where(x => !x.IsDeleted)
            .ToListAsync();

        return Ok(new BootstrapSyncResponse
        {
            Booths = booths,
            MenuItems = menuItems
        });
    }
}