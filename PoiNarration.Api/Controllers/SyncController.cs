using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoiNarration.Api.Data;
using PoiNarration.Core.Models; // Bắt buộc phải có dòng này để lấy cái khuôn

namespace PoiNarration.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SyncController : ControllerBase
{
    private readonly AppDbContext _db;

    public SyncController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("bootstrap")]
    // SỬA Ở ĐÂY: Khai báo rõ kiểu trả về là BootstrapSyncResponse
    public async Task<ActionResult<BootstrapSyncResponse>> Bootstrap()
    {
        var response = new BootstrapSyncResponse();

        // 1. Kéo dữ liệu Trạm
        response.Booths = await _db.Booths
            .OrderBy(x => x.Priority)
            .ToListAsync();

        // 2. Kéo dữ liệu Menu (chỉ lấy món chưa bị xóa)
        response.MenuItems = await _db.BoothMenuItems
            .Where(x => !x.IsDeleted)
            .ToListAsync();

        // 3. Kéo dữ liệu Dịch thuật
        response.BoothTranslations = await _db.BoothTranslations.ToListAsync();
        response.MenuTranslations = await _db.BoothMenuItemTranslations.ToListAsync();

        // (Nếu Database của bạn không có bảng Zone thì kệ nó, bên class nó tự gán bằng List rỗng rồi)

        // Trả đúng cái khuôn ra
        return Ok(response);
    }
}