using Microsoft.AspNetCore.Mvc;
using PoiNarration.Api.Data;
using PoiNarration.Core.Models; // Sử dụng Model và DTO từ Core
using Microsoft.EntityFrameworkCore;

namespace PoiNarration.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlaybackLogsController : ControllerBase
{
    private readonly AppDbContext _db;

    public PlaybackLogsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PlaybackLogRequest request) // Đã đổi sang PlaybackLogRequest từ Core
    {
        if (string.IsNullOrWhiteSpace(request.BoothId))
            return BadRequest("BoothId là bắt buộc.");

        // Tạo Entity PlaybackLog (đã được chuyển sang Core)
        var log = new PlaybackLog
        {
            BoothId = request.BoothId,
            TriggerType = request.TriggerType,
            Language = request.Language,
            DurationSeconds = request.DurationSeconds,
            Lat = request.Lat,
            Lng = request.Lng,
            IsCompleted = request.IsCompleted,
            SessionId = request.SessionId,
            PlayedAtUtc = DateTime.UtcNow,
            IsSynced = true // Vì lưu trực tiếp trên Server nên mặc định là đã Sync
        };

        _db.PlaybackLogs.Add(log);
        await _db.SaveChangesAsync();

        return Ok(log);
    }
}