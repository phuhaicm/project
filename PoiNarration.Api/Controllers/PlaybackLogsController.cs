using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoiNarration.Api.Data;
using PoiNarration.Core.Models;

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
    public async Task<IActionResult> Create([FromBody] PlaybackLogRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.BoothId))
            return BadRequest("BoothId là bắt buộc.");

        var log = new PlaybackLog
        {
            VisitorUserId = request.VisitorUserId,
            BoothId = request.BoothId,
            TriggerType = request.TriggerType,
            Language = request.Language,
            DurationSeconds = request.DurationSeconds,
            Lat = request.Lat,
            Lng = request.Lng,
            IsCompleted = request.IsCompleted,
            SessionId = request.SessionId,
            PlayedAtUtc = DateTime.UtcNow,
            IsSynced = true
        };

        _db.PlaybackLogs.Add(log);

        // THÊM MỚI: cập nhật thời gian hoạt động cuối của visitor
        if (!string.IsNullOrWhiteSpace(request.VisitorUserId))
        {
            var visitor = await _db.VisitorUsers
                .FirstOrDefaultAsync(x => x.Id == request.VisitorUserId);

            if (visitor != null)
            {
                visitor.LastActiveAtUtc = DateTime.UtcNow;
            }
        }

        await _db.SaveChangesAsync();

        return Ok(log);
    }
}