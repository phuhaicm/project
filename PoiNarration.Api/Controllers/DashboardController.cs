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
        // Mình bắt đầu từ bảng Booths, sau đó đếm số Log tương ứng
        var result = await _db.Booths
            .Select(b => new
            {
                BoothId = b.Id,
                BoothName = b.NameVi ?? "Chưa đặt tên", // Tránh lỗi null tên
                Count = _db.PlaybackLogs.Count(log => log.BoothId == b.Id)
            })
            .OrderByDescending(x => x.Count) // Thằng nào nghe nhiều nhất lên đầu
            .Take(10)
            .ToListAsync();
        return Ok(result);
    }
}
