using Microsoft.AspNetCore.Mvc;
using PoiNarration.Api.Data;
using PoiNarration.Core.Models;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class BoothMenuController : ControllerBase
{
    private readonly AppDbContext _context;

    public BoothMenuController(AppDbContext context)
    {
        _context = context;
    }

    // 1. LẤY DANH SÁCH MÓN ĂN
    // URL: GET http://localhost:5174/api/BoothMenu/123
    [HttpGet("{boothId}")]
    public async Task<ActionResult<IEnumerable<BoothMenuItem>>> GetMenu(string boothId)
    {
        var menu = await _context.BoothMenuItems
                                 .Where(m => m.BoothId == boothId)
                                 .ToListAsync();
        return Ok(menu);
    }

    // 2. THÊM MÓN ĂN MỚI
    // URL: POST http://localhost:5174/api/BoothMenu/123
    [HttpPost("{boothId}")] // Phải thêm {boothId} vào đây để nó khớp với tham số
    public async Task<ActionResult<BoothMenuItem>> AddMenuItem(string boothId, [FromBody] BoothMenuItem item)
    {
        item.BoothId = boothId;
        _context.BoothMenuItems.Add(item);
        await _context.SaveChangesAsync();
        return Ok(item);
    }

    // 3. XÓA MÓN ĂN
    // URL: DELETE http://localhost:5174/api/BoothMenu/123/items/abc
    [HttpDelete("{boothId}/items/{itemId}")] // Định nghĩa rõ ràng để tránh xung đột
    public async Task<IActionResult> DeleteMenuItem(string boothId, string itemId)
    {
        var item = await _context.BoothMenuItems
                                 .FirstOrDefaultAsync(m => m.Id == itemId && m.BoothId == boothId);

        if (item == null) return NotFound("Không tìm thấy món ăn này.");

        _context.BoothMenuItems.Remove(item);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Đã xóa thành công!" });
    }
}