using Microsoft.EntityFrameworkCore;
using PoiNarration.Api.Models.Entities; // Quay lại dùng đồ nhà làm cho lành
namespace PoiNarration.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<PoiNarration.Api.Models.Entities.Booth> Booths => Set<PoiNarration.Api.Models.Entities.Booth>();
    public DbSet<BoothTranslation> BoothTranslations => Set<BoothTranslation>();
    public DbSet<BoothMenuItemTranslation> BoothMenuItemTranslations => Set<BoothMenuItemTranslation>();

    public DbSet<BoothMenuItem> BoothMenuItems => Set<BoothMenuItem>();
    public DbSet<PlaybackLog> PlaybackLogs => Set<PlaybackLog>();
    public DbSet<AppUser> AppUsers => Set<AppUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Booth>().HasKey(x => x.Id);
        modelBuilder.Entity<BoothMenuItem>().HasKey(x => x.Id);
        modelBuilder.Entity<PlaybackLog>().HasKey(x => x.Id);

        modelBuilder.Entity<BoothMenuItem>()
            .HasIndex(x => new { x.BoothId, x.IsDeleted });

        modelBuilder.Entity<PlaybackLog>()
            .HasIndex(x => new { x.BoothId, x.PlayedAtUtc });
    }
}