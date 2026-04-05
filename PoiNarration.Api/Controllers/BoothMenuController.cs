using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoiNarration.Api.Data;
using PoiNarration.Api.DTOs.Menu;
using PoiNarration.Api.Models.Entities;

namespace PoiNarration.Api.Controllers;

[ApiController]
[Route("api/boothmenu")] // Đổi lại Route cho khớp với Web
public class BoothMenuController : ControllerBase
{
    private readonly AppDbContext _db;

    public BoothMenuController(AppDbContext db)
    {
        _db = db;
    }

    // 1. Lấy danh sách món ăn: GET /api/boothmenu/booth-01
    [HttpGet("{boothId}")]
    public async Task<ActionResult<List<BoothMenuItem>>> GetByBooth(string boothId)
    {
        var items = await _db.BoothMenuItems
            .Where(x => x.BoothId == boothId && !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync();

        return Ok(items);
    }

    // 2. Thêm món mới: POST /api/boothmenu/booth-01
    [HttpPost("{boothId}")]
    public async Task<ActionResult<BoothMenuItem>> Create(string boothId, [FromBody] UpsertMenuItemRequest request)
    {
        // Kiểm tra xem Booth có tồn tại không (tùy chọn)
        var boothExists = await _db.Booths.AnyAsync(x => x.Id == boothId);
        if (!boothExists)
            return NotFound("Không tìm thấy mã gian hàng này trong hệ thống.");

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

    // 3. Cập nhật món: PUT /api/boothmenu/booth-01/items/menuId
    [HttpPut("{boothId}/items/{menuId}")]
    public async Task<ActionResult<BoothMenuItem>> Update(string boothId, string menuId, [FromBody] UpsertMenuItemRequest request)
    {
        var item = await _db.BoothMenuItems
            .FirstOrDefaultAsync(x => x.Id == menuId && x.BoothId == boothId);

        if (item == null)
            return NotFound("Không tìm thấy món ăn cần sửa.");

        item.Name = request.Name;
        item.Description = request.Description;
        item.Price = request.Price;
        item.ImageUrl = request.ImageUrl;
        item.UpdatedAtUtc = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Ok(item);
    }

    // 4. Xóa món: DELETE /api/boothmenu/booth-01/items/menuId
    [HttpDelete("{boothId}/items/{menuId}")]
    public async Task<IActionResult> Delete(string boothId, string menuId)
    {
        var item = await _db.BoothMenuItems
            .FirstOrDefaultAsync(x => x.Id == menuId && x.BoothId == boothId);

        if (item == null)
            return NotFound();

        // Xóa mềm (Soft Delete)
        item.IsDeleted = true;
        item.UpdatedAtUtc = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return NoContent();
    }
}