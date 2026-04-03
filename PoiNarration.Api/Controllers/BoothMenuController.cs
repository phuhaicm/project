using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoiNarration.Api.Data;
using PoiNarration.Api.DTOs.Menu;
using PoiNarration.Api.Models.Entities;

namespace PoiNarration.Api.Controllers;

[ApiController]
[Route("api/booths/{boothId}/menu")]
public class BoothMenuController : ControllerBase
{
    private readonly AppDbContext _db;

    public BoothMenuController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<BoothMenuItem>>> GetByBooth(string boothId)
    {
        var items = await _db.BoothMenuItems
            .Where(x => x.BoothId == boothId && !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync();

        return Ok(items);
    }

    [HttpPost]
    public async Task<ActionResult<BoothMenuItem>> Create(string boothId, [FromBody] UpsertMenuItemRequest request)
    {
        var boothExists = await _db.Booths.AnyAsync(x => x.Id == boothId);
        if (!boothExists)
            return NotFound("Không tìm thấy booth.");

        var item = new BoothMenuItem
        {
            BoothId = boothId,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            ImageUrl = request.ImageUrl,
            UpdatedAtUtc = DateTime.UtcNow,
            IsDeleted = false
        };

        _db.BoothMenuItems.Add(item);
        await _db.SaveChangesAsync();

        return Ok(item);
    }

    [HttpPut("{menuId}")]
    public async Task<ActionResult<BoothMenuItem>> Update(string boothId, string menuId, [FromBody] UpsertMenuItemRequest request)
    {
        var item = await _db.BoothMenuItems
            .FirstOrDefaultAsync(x => x.Id == menuId && x.BoothId == boothId);

        if (item == null)
            return NotFound("Không tìm thấy menu item.");

        item.Name = request.Name;
        item.Description = request.Description;
        item.Price = request.Price;
        item.ImageUrl = request.ImageUrl;
        item.UpdatedAtUtc = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Ok(item);
    }

    [HttpDelete("{menuId}")]
    public async Task<IActionResult> Delete(string boothId, string menuId)
    {
        var item = await _db.BoothMenuItems
            .FirstOrDefaultAsync(x => x.Id == menuId && x.BoothId == boothId);

        if (item == null)
            return NotFound();

        item.IsDeleted = true;
        item.UpdatedAtUtc = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return NoContent();
    }
}