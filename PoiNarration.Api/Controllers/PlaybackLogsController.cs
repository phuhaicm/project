using Microsoft.AspNetCore.Mvc;
using PoiNarration.Api.Data;
using PoiNarration.Api.DTOs.PlaybackLogs;
using PoiNarration.Api.Models.Entities;

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
    public async Task<IActionResult> Create([FromBody] CreatePlaybackLogRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.BoothId))
            return BadRequest("BoothId là bắt buộc.");

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
            PlayedAtUtc = DateTime.UtcNow
        };

        _db.PlaybackLogs.Add(log);
        await _db.SaveChangesAsync();

        return Ok(log);
    }
}