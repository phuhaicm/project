using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoiNarration.Api.Data; // Nhớ để ý namespace này có khớp với thư mục Data của bạn không

namespace PoiNarration.Api.Controllers
{
    // Giữ cái khuôn mẫu MenuItem ở đây hoặc chuyển sang file riêng trong folder Models
    public class MenuItem
    {
        public string? Id { get; set; }
        public string? BoothId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class BoothMenuController : ControllerBase
    {
        private readonly AppDbContext _db;

        // TIÊM (Inject) Database vào Controller để sử dụng
        public BoothMenuController(AppDbContext db)
        {
            _db = db;
        }

        // 1. Lấy danh sách món ăn từ Database thật
        [HttpGet("{boothId}")]
        public async Task<IActionResult> GetMenu(string boothId)
        {
            var menu = await _db.MenuItems
                                .Where(m => m.BoothId == boothId)
                                .ToListAsync();
            return Ok(menu);
        }

        // 2. Thêm món mới và LƯU VĨNH VIỄN vào file .db
        [HttpPost("{boothId}")]
        public async Task<IActionResult> AddMenuItem(string boothId, [FromBody] MenuItem newItem)
        {
            // Tự tạo ID nếu bên Web không gửi
            if (string.IsNullOrEmpty(newItem.Id))
                newItem.Id = Guid.NewGuid().ToString();

            newItem.BoothId = boothId;

            _db.MenuItems.Add(newItem); // Cho món ăn vào giỏ
            await _db.SaveChangesAsync(); // Chốt đơn, lưu xuống ổ cứng!

            return Ok(new { Message = "Đã lưu vào Database thật thành công!" });
        }

        // 3. Xóa món ăn khỏi Database
        [HttpDelete("{boothId}/items/{itemId}")]
        public async Task<IActionResult> DeleteMenuItem(string boothId, string itemId)
        {
            var item = await _db.MenuItems
                                .FirstOrDefaultAsync(m => m.BoothId == boothId && m.Id == itemId);

            if (item != null)
            {
                _db.MenuItems.Remove(item);
                await _db.SaveChangesAsync(); // Lưu lại thay đổi sau khi xóa
                return Ok(new { Message = "Đã xóa xong khỏi Database!" });
            }
            return NotFound("Không tìm thấy món để xóa");
        }
    }
}