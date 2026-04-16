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
        var result = await _db.PlaybackLogs
            .GroupBy(x => x.BoothId)
            .Select(g => new
            {
                BoothId = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .ToListAsync();

        var boothIds = result.Select(x => x.BoothId).ToList();

        var boothMap = await _db.Booths
            .Where(x => boothIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, x => x.NameVi);

        var data = result.Select(x => new
        {
            boothId = x.BoothId,
            boothName = boothMap.ContainsKey(x.BoothId) ? boothMap[x.BoothId] : x.BoothId,
            count = x.Count
        });

        return Ok(data);
    }
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var todayUtc = DateTime.UtcNow.Date;
        var onlineThreshold = DateTime.UtcNow.AddMinutes(-5);

        var totalBooths = await _db.Booths.CountAsync();
        var totalOwners = await _db.AppUsers.CountAsync(x => x.Role == "Owner");
        var totalPlaybackLogs = await _db.PlaybackLogs.CountAsync();
        var playbackToday = await _db.PlaybackLogs.CountAsync(x => x.PlayedAtUtc >= todayUtc);

        var avgDuration = await _db.PlaybackLogs.AnyAsync()
            ? await _db.PlaybackLogs.AverageAsync(x => (double?)x.DurationSeconds) ?? 0
            : 0;

        // THÊM MỚI - user analytics
        var totalVisitors = await _db.VisitorUsers.CountAsync();
        var activeVisitorsToday = await _db.VisitorUsers.CountAsync(x => x.LastActiveAtUtc >= todayUtc);
        var onlineVisitors = await _db.VisitorUsers.CountAsync(x => x.LastActiveAtUtc >= onlineThreshold);
        var totalBoothVisits = await _db.BoothVisitLogs.CountAsync();

        var dto = new PoiNarration.Api.DTOs.Dashboard.DashboardSummaryDto
        {
            TotalBooths = totalBooths,
            TotalOwners = totalOwners,
            TotalPlaybackLogs = totalPlaybackLogs,
            PlaybackToday = playbackToday,
            AverageDurationSeconds = avgDuration,

            TotalVisitors = totalVisitors,
            ActiveVisitorsToday = activeVisitorsToday,
            OnlineVisitors = onlineVisitors,
            TotalBoothVisits = totalBoothVisits
        };

        return Ok(dto);
    }

    [HttpGet("latest-logs")]
    public async Task<IActionResult> GetLatestLogs()
    {
        var logs = await _db.PlaybackLogs
            .OrderByDescending(x => x.PlayedAtUtc)
            .Take(10)
            .ToListAsync();

        var boothIds = logs.Select(x => x.BoothId).Distinct().ToList();

        var boothMap = await _db.Booths
            .Where(x => boothIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, x => x.NameVi);

        var result = logs.Select(x => new
        {
            id = x.Id,
            boothId = x.BoothId,
            boothName = boothMap.ContainsKey(x.BoothId) ? boothMap[x.BoothId] : x.BoothId,
            triggerType = x.TriggerType,
            language = x.Language,
            durationSeconds = x.DurationSeconds,
            playedAtUtc = x.PlayedAtUtc
        });

        return Ok(result);
    }
}
