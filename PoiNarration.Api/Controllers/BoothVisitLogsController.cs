using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoiNarration.Api.Data;
using PoiNarration.Core.Models;

namespace PoiNarration.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoothVisitLogsController : ControllerBase
{
    private readonly AppDbContext _db;

    public BoothVisitLogsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BoothVisitLog request)
    {
        if (string.IsNullOrWhiteSpace(request.VisitorUserId))
            return BadRequest("VisitorUserId là bắt buộc.");

        if (string.IsNullOrWhiteSpace(request.BoothId))
            return BadRequest("BoothId là bắt buộc.");

        request.Id = 0;
        request.IsSynced = true;
        request.VisitedAtUtc = request.VisitedAtUtc == default
            ? DateTime.UtcNow
            : request.VisitedAtUtc;

        _db.BoothVisitLogs.Add(request);

        // THÊM MỚI: cập nhật thời gian hoạt động cuối của visitor
        var visitor = await _db.VisitorUsers
            .FirstOrDefaultAsync(x => x.Id == request.VisitorUserId);

        if (visitor != null)
        {
            visitor.LastActiveAtUtc = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync();

        return Ok(request);
    }

    [HttpPost("batch")]
    public async Task<IActionResult> CreateBatch([FromBody] List<BoothVisitLog> requests)
    {
        if (requests == null || requests.Count == 0)
            return BadRequest("Danh sách BoothVisitLog rỗng.");

        var now = DateTime.UtcNow;

        foreach (var item in requests)
        {
            item.Id = 0;
            item.IsSynced = true;

            if (item.VisitedAtUtc == default)
                item.VisitedAtUtc = now;
        }

        _db.BoothVisitLogs.AddRange(requests);

        // THÊM MỚI: cập nhật LastActiveAtUtc cho tất cả visitor có trong batch
        var visitorIds = requests
            .Where(x => !string.IsNullOrWhiteSpace(x.VisitorUserId))
            .Select(x => x.VisitorUserId)
            .Distinct()
            .ToList();

        if (visitorIds.Any())
        {
            var visitors = await _db.VisitorUsers
                .Where(x => visitorIds.Contains(x.Id))
                .ToListAsync();

            foreach (var visitor in visitors)
            {
                visitor.LastActiveAtUtc = now;
            }
        }

        await _db.SaveChangesAsync();

        return Ok(new { Count = requests.Count });
    }
}
