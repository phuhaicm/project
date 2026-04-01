using Microsoft.EntityFrameworkCore;
using PoiNarration.Api.Controllers; // Để nó nhận ra class MenuItem bạn viết lúc nãy

namespace PoiNarration.Api.Data
{
    // AppDbContext đóng vai trò là "Người quản kho" 
    // Nó kết nối giữa các Class C# và các Bảng trong Database
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Khai báo bảng MenuItems dựa trên khuôn mẫu là class MenuItem
        public DbSet<MenuItem> MenuItems { get; set; }

        // Bạn có thể thêm các DbSet khác ở đây nếu sau này có thêm bảng Booth, Zone...
    }
}