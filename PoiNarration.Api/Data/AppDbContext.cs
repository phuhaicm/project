using Microsoft.EntityFrameworkCore;
using PoiNarration.Api.Models.Entities; // Quay lại dùng đồ nhà làm cho lành
namespace PoiNarration.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Booth> Booths => Set<Booth>();
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