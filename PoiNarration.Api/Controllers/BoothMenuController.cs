using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoiNarration.Api.Data;
using PoiNarration.Api.DTOs.Menu;
using PoiNarration.Api.Services;
using PoiNarration.Core.Models; // Sử dụng duy nhất nguồn Core

namespace PoiNarration.Api.Controllers;

[ApiController]
[Route("api/boothmenu")]
public class BoothMenuController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ITranslationService _translationService;

    // Hợp nhất thành một Constructor duy nhất
    public BoothMenuController(AppDbContext db, ITranslationService translationService)
    {
        _db = db;
        _translationService = translationService;
    }

    // 1. Lấy danh sách món ăn
    [HttpGet("{boothId}")]
    public async Task<ActionResult<List<BoothMenuItem>>> GetByBooth(string boothId)
    {
        var items = await _db.BoothMenuItems
            .Where(x => x.BoothId == boothId && !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync();

        return Ok(items);
    }

    // 2. Thêm món mới
    [HttpPost("{boothId}")]
    public async Task<ActionResult<BoothMenuItem>> Create(string boothId, [FromBody] UpsertMenuItemRequest request)
    {
        var boothExists = await _db.Booths.AnyAsync(x => x.Id == boothId);
        if (!boothExists)
            return NotFound("Không tìm thấy mã gian hàng này trong hệ thống.");

        var item = new BoothMenuItem
        {
            Id = Guid.NewGuid().ToString(),
            BoothId = boothId,
            Name = request.Name,
            NameEn = request.NameEn,
            Description = request.Description,
            DescriptionEn = request.DescriptionEn,
            Price = request.Price,
            PriceUsd = request.PriceUsd,
            ImageUrl = request.ImageUrl,
            UpdatedAtUtc = DateTime.UtcNow,
            IsDeleted = false
        };

        _db.BoothMenuItems.Add(item);
        await _db.SaveChangesAsync();

        // --- THÊM THEO YÊU CẦU ---
        var translations = await _translationService.BuildMenuTranslationsAsync(item);
        _db.BoothMenuItemTranslations.AddRange(translations);

        await _db.SaveChangesAsync();
        return Ok(item);
        // ------------------------
    }

    // 3. Cập nhật món
    [HttpPut("{boothId}/items/{menuId}")]
    public async Task<ActionResult<BoothMenuItem>> Update(string boothId, string menuId, [FromBody] UpsertMenuItemRequest request)
    {
        var item = await _db.BoothMenuItems
            .FirstOrDefaultAsync(x => x.Id == menuId && x.BoothId == boothId);

        if (item == null)
            return NotFound("Không tìm thấy món ăn cần sửa.");

        item.Name = request.Name;
        item.NameEn = request.NameEn;
        item.Description = request.Description;
        item.DescriptionEn = request.DescriptionEn;
        item.Price = request.Price;
        item.PriceUsd = request.PriceUsd;
        item.ImageUrl = request.ImageUrl;
        item.UpdatedAtUtc = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        // --- THÊM THEO YÊU CẦU ---
        var oldTranslations = _db.BoothMenuItemTranslations.Where(x => x.MenuItemId == item.Id);
        _db.BoothMenuItemTranslations.RemoveRange(oldTranslations);

        await _db.SaveChangesAsync();

        var newTranslations = await _translationService.BuildMenuTranslationsAsync(item);
        _db.BoothMenuItemTranslations.AddRange(newTranslations);

        await _db.SaveChangesAsync();
        return Ok(item);
        // ------------------------
    }

    // 4. Xóa món (Soft Delete)
    [HttpDelete("{boothId}/items/{menuId}")]
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