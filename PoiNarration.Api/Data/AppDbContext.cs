using Microsoft.EntityFrameworkCore;
using PoiNarration.Core.Models; // Để nó hiểu model BoothMenuItem

namespace PoiNarration.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Khai báo bảng chứa thực đơn
        public DbSet<BoothMenuItem> BoothMenuItems { get; set; }
    }
}