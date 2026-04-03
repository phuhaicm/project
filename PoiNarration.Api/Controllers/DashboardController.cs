using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoiNarration.Api.Data;

namespace PoiNarration.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _db;

    public DashboardController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("top-booths")]
    public async Task<IActionResult> GetTopBooths()
    {
        var result = await (
            from log in _db.PlaybackLogs
            group log by log.BoothId into g
            join booth in _db.Booths on g.Key equals booth.Id
            orderby g.Count() descending
            select new
            {
                BoothId = booth.Id,
                BoothName = booth.NameVi,
                Count = g.Count()
            }
        ).Take(10).ToListAsync();

        return Ok(result);
    }
}