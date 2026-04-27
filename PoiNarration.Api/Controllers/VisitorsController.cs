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
    [HttpPut("{visitorId}/language")]
    public async Task<IActionResult> UpdateLanguage(string visitorId, [FromBody] UpdateVisitorLanguageRequest request)
    {
        if (string.IsNullOrWhiteSpace(visitorId))
            return BadRequest("visitorId là bắt buộc.");

        var visitor = await _db.VisitorUsers.FirstOrDefaultAsync(x => x.Id == visitorId);
        if (visitor == null)
            return NotFound("Không tìm thấy visitor.");

        visitor.PreferredLanguage = string.IsNullOrWhiteSpace(request.PreferredLanguage)
            ? visitor.PreferredLanguage
            : request.PreferredLanguage;

        visitor.LastActiveAtUtc = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Ok(new
        {
            visitor.Id,
            visitor.VisitorCode,
            visitor.PreferredLanguage,
            visitor.LastActiveAtUtc
        });
    }


    [HttpPut("{visitorId}/touch")]
    public async Task<IActionResult> Touch(string visitorId)
    {
        if (string.IsNullOrWhiteSpace(visitorId))
            return BadRequest("visitorId là bắt buộc.");

        var visitor = await _db.VisitorUsers.FirstOrDefaultAsync(x => x.Id == visitorId);
        if (visitor == null)
            return NotFound("Không tìm thấy visitor.");

        visitor.LastActiveAtUtc = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return Ok(new
        {
            visitor.Id,
            visitor.VisitorCode,
            visitor.LastActiveAtUtc
        });
    }
    private static string BuildVisitorCode()
    {
        return $"VIS-{Guid.NewGuid():N}".Substring(0, 10).ToUpper();
    }
    [HttpGet("activity-details")]
    public async Task<IActionResult> GetActivityDetails()
    {
        var onlineThreshold = DateTime.UtcNow.AddMinutes(-5);

        var boothNames = await _db.Booths
            .AsNoTracking()
            .ToDictionaryAsync(
                x => x.Id,
                x => string.IsNullOrWhiteSpace(x.NameVi) ? x.NameEn : x.NameVi);

        var visitors = await _db.VisitorUsers
            .AsNoTracking()
            .OrderByDescending(x => x.LastActiveAtUtc)
            .ToListAsync();

        var visitGroups = await _db.BoothVisitLogs
            .AsNoTracking()
            .GroupBy(x => new { x.VisitorUserId, x.BoothId })
            .Select(g => new VisitorBoothActivityDto
            {
                VisitorUserId = g.Key.VisitorUserId,
                BoothId = g.Key.BoothId,
                Count = g.Count(),
                LastAtUtc = g.Max(x => x.VisitedAtUtc)
            })
            .ToListAsync();

        var playbackGroups = await _db.PlaybackLogs
            .AsNoTracking()
            .Where(x => !string.IsNullOrWhiteSpace(x.VisitorUserId))
            .GroupBy(x => new { VisitorUserId = x.VisitorUserId!, x.BoothId })
            .Select(g => new VisitorBoothActivityDto
            {
                VisitorUserId = g.Key.VisitorUserId,
                BoothId = g.Key.BoothId,
                Count = g.Count(),
                LastAtUtc = g.Max(x => x.PlayedAtUtc)
            })
            .ToListAsync();

        var result = visitors.Select(v =>
        {
            var visitedBooths = visitGroups
                .Where(x => x.VisitorUserId == v.Id)
                .Select(x => new VisitorBoothActivityDto
                {
                    VisitorUserId = x.VisitorUserId,
                    BoothId = x.BoothId,
                    BoothName = boothNames.TryGetValue(x.BoothId, out var boothName) ? boothName : x.BoothId,
                    Count = x.Count,
                    LastAtUtc = x.LastAtUtc
                })
                .OrderByDescending(x => x.LastAtUtc)
                .ToList();

            var playedBooths = playbackGroups
                .Where(x => x.VisitorUserId == v.Id)
                .Select(x => new VisitorBoothActivityDto
                {
                    VisitorUserId = x.VisitorUserId,
                    BoothId = x.BoothId,
                    BoothName = boothNames.TryGetValue(x.BoothId, out var boothName) ? boothName : x.BoothId,
                    Count = x.Count,
                    LastAtUtc = x.LastAtUtc
                })
                .OrderByDescending(x => x.LastAtUtc)
                .ToList();

            return new VisitorActivityDetailResponse
            {
                VisitorId = v.Id,
                VisitorCode = v.VisitorCode,
                DisplayName = v.DisplayName,
                PreferredLanguage = v.PreferredLanguage,
                Platform = v.Platform,
                AppVersion = v.AppVersion,
                LastActiveAtUtc = v.LastActiveAtUtc,
                IsOnline = v.LastActiveAtUtc >= onlineThreshold,
                TotalVisitedBooths = visitedBooths.Sum(x => x.Count),
                TotalPlayedBooths = playedBooths.Sum(x => x.Count),
                VisitedBooths = visitedBooths,
                PlayedBooths = playedBooths
            };
        }).ToList();

        return Ok(result);
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
public class UpdateVisitorLanguageRequest
{
    public string PreferredLanguage { get; set; } = "vi";
}
public class VisitorActivityDetailResponse
{
    public string VisitorId { get; set; } = "";
    public string VisitorCode { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public string PreferredLanguage { get; set; } = "vi";
    public string? Platform { get; set; }
    public string? AppVersion { get; set; }
    public DateTime LastActiveAtUtc { get; set; }
    public bool IsOnline { get; set; }

    public int TotalVisitedBooths { get; set; }
    public int TotalPlayedBooths { get; set; }

    public List<VisitorBoothActivityDto> VisitedBooths { get; set; } = new();
    public List<VisitorBoothActivityDto> PlayedBooths { get; set; } = new();
}

public class VisitorBoothActivityDto
{
    public string VisitorUserId { get; set; } = "";
    public string BoothId { get; set; } = "";
    public string BoothName { get; set; } = "";
    public int Count { get; set; }
    public DateTime LastAtUtc { get; set; }
}