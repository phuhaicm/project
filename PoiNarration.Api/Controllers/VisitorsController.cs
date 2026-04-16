using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoiNarration.Api.Data;
using PoiNarration.Core.Models;

namespace PoiNarration.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VisitorsController : ControllerBase
{
    private readonly AppDbContext _db;

    public VisitorsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] VisitorRegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.DeviceKey))
            return BadRequest("DeviceKey là bắt buộc.");

        var existing = await _db.VisitorUsers
            .FirstOrDefaultAsync(x => x.DeviceKey == request.DeviceKey);

        if (existing != null)
        {
            existing.LastActiveAtUtc = DateTime.UtcNow;
            if (!string.IsNullOrWhiteSpace(request.PreferredLanguage))
                existing.PreferredLanguage = request.PreferredLanguage;
            if (!string.IsNullOrWhiteSpace(request.AppVersion))
                existing.AppVersion = request.AppVersion;
            if (!string.IsNullOrWhiteSpace(request.Platform))
                existing.Platform = request.Platform;

            await _db.SaveChangesAsync();

            return Ok(new VisitorRegisterResponse
            {
                VisitorId = existing.Id,
                VisitorCode = existing.VisitorCode,
                DisplayName = existing.DisplayName,
                PreferredLanguage = existing.PreferredLanguage
            });
        }

        var code = BuildVisitorCode();

        var visitor = new VisitorUser
        {
            Id = Guid.NewGuid().ToString(),
            VisitorCode = code,
            DisplayName = $"Khách {code}",
            DeviceKey = request.DeviceKey,
            PreferredLanguage = string.IsNullOrWhiteSpace(request.PreferredLanguage) ? "vi" : request.PreferredLanguage,
            Platform = request.Platform,
            AppVersion = request.AppVersion,
            CreatedAtUtc = DateTime.UtcNow,
            LastActiveAtUtc = DateTime.UtcNow,
            IsActive = true
        };

        _db.VisitorUsers.Add(visitor);
        await _db.SaveChangesAsync();

        return Ok(new VisitorRegisterResponse
        {
            VisitorId = visitor.Id,
            VisitorCode = visitor.VisitorCode,
            DisplayName = visitor.DisplayName,
            PreferredLanguage = visitor.PreferredLanguage
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var visitors = await _db.VisitorUsers
            .OrderByDescending(x => x.LastActiveAtUtc)
            .ToListAsync();

        return Ok(visitors);
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var totalVisitors = await _db.VisitorUsers.CountAsync();
        var totalVisitLogs = await _db.BoothVisitLogs.CountAsync();
        var totalPlaybackLogs = await _db.PlaybackLogs.CountAsync();
        var todayUtc = DateTime.UtcNow.Date;
        var onlineThreshold = DateTime.UtcNow.AddMinutes(-5);

        var activeVisitorsToday = await _db.VisitorUsers.CountAsync(x => x.LastActiveAtUtc >= todayUtc);
        var onlineVisitors = await _db.VisitorUsers.CountAsync(x => x.LastActiveAtUtc >= onlineThreshold);

        var topLanguages = await _db.VisitorUsers
            .GroupBy(x => x.PreferredLanguage)
            .Select(g => new { Language = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ToListAsync();

        var topBoothsByVisit = await _db.BoothVisitLogs
            .GroupBy(x => x.BoothId)
            .Select(g => new { BoothId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(10)
            .ToListAsync();

        var topBoothsByPlayback = await _db.PlaybackLogs
            .GroupBy(x => x.BoothId)
            .Select(g => new { BoothId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(10)
            .ToListAsync();

        return Ok(new
        {
            TotalVisitors = totalVisitors,
            TotalVisitLogs = totalVisitLogs,
            TotalPlaybackLogs = totalPlaybackLogs,
            ActiveVisitorsToday = activeVisitorsToday,
            OnlineVisitors = onlineVisitors,
            TopLanguages = topLanguages,
            TopBoothsByVisit = topBoothsByVisit,
            TopBoothsByPlayback = topBoothsByPlayback
        });

    }

    private static string BuildVisitorCode()
    {
        return $"VIS-{Guid.NewGuid():N}".Substring(0, 10).ToUpper();
    }
}

public class VisitorRegisterRequest
{
    public string DeviceKey { get; set; } = "";
    public string PreferredLanguage { get; set; } = "vi";
    public string? Platform { get; set; }
    public string? AppVersion { get; set; }
}

public class VisitorRegisterResponse
{
    public string VisitorId { get; set; } = "";
    public string VisitorCode { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public string PreferredLanguage { get; set; } = "vi";
}
