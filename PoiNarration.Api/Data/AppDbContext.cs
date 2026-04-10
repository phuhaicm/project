using Microsoft.EntityFrameworkCore;
// 1. Chỉ nên ưu tiên dùng Core cho các bảng đồng bộ với Mobile
using PoiNarration.Core.Models;

namespace PoiNarration.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // CHỈ GIỮ LẠI MỘT BỘ DUY NHẤT - Dùng Model từ Core
    public DbSet<Booth> Booths => Set<Booth>();
    public DbSet<BoothMenuItem> BoothMenuItems => Set<BoothMenuItem>();

    // Nếu trong Core bạn đặt tên là BoothTranslationLocal thì dùng tên đó
    public DbSet<BoothTranslationLocal> BoothTranslations => Set<BoothTranslationLocal>();
    public DbSet<BoothMenuItemTranslationLocal> BoothMenuItemTranslations => Set<BoothMenuItemTranslationLocal>();
    // Đảm bảo dòng này trỏ đúng vào class Tour của Core
    public DbSet<Tour> Tours => Set<Tour>();
    public DbSet<TourBooth> TourBooths => Set<TourBooth>();
    // Những bảng chỉ API dùng, không share cho Mobile thì dùng Entities
    public DbSet<PlaybackLog> PlaybackLogs => Set<PlaybackLog>();
    public DbSet<PoiNarration.Api.Models.Entities.AppUser> AppUsers => Set<PoiNarration.Api.Models.Entities.AppUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Cấu hình khóa chính cho Tour (Lấy từ class Tour ở Core)
        modelBuilder.Entity<Tour>().HasKey(x => x.Id);
        modelBuilder.Entity<TourBooth>().HasKey(x => x.Id);
        // Chỉ định rõ Key cho các Model từ Core
        modelBuilder.Entity<Booth>().HasKey(x => x.Id);
        modelBuilder.Entity<BoothMenuItem>().HasKey(x => x.Id);

        // Model chỉ có ở API
        modelBuilder.Entity<PlaybackLog>().HasKey(x => x.Id);
        modelBuilder.Entity<BoothMenuItem>()
            .HasIndex(x => new { x.BoothId, x.IsDeleted });

        modelBuilder.Entity<PlaybackLog>()
            .HasIndex(x => new { x.BoothId, x.PlayedAtUtc });
    }
}