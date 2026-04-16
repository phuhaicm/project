using Microsoft.EntityFrameworkCore;
using PoiNarration.Core.Models;

namespace PoiNarration.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Booth> Booths => Set<Booth>();
    public DbSet<BoothMenuItem> BoothMenuItems => Set<BoothMenuItem>();
    public DbSet<BoothTranslationLocal> BoothTranslations => Set<BoothTranslationLocal>();
    public DbSet<BoothMenuItemTranslationLocal> BoothMenuItemTranslations => Set<BoothMenuItemTranslationLocal>();
    public DbSet<Tour> Tours => Set<Tour>();
    public DbSet<TourBooth> TourBooths => Set<TourBooth>();

    public DbSet<PlaybackLog> PlaybackLogs => Set<PlaybackLog>();
    public DbSet<VisitorUser> VisitorUsers => Set<VisitorUser>();         // THÊM MỚI
    public DbSet<BoothVisitLog> BoothVisitLogs => Set<BoothVisitLog>();   // THÊM MỚI

    public DbSet<PoiNarration.Api.Models.Entities.AppUser> AppUsers
        => Set<PoiNarration.Api.Models.Entities.AppUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tour>().HasKey(x => x.Id);
        modelBuilder.Entity<TourBooth>().HasKey(x => x.Id);

        modelBuilder.Entity<Booth>().HasKey(x => x.Id);
        modelBuilder.Entity<BoothMenuItem>().HasKey(x => x.Id);
        modelBuilder.Entity<PlaybackLog>().HasKey(x => x.Id);

        modelBuilder.Entity<VisitorUser>().HasKey(x => x.Id);
        modelBuilder.Entity<BoothVisitLog>().HasKey(x => x.Id);

        modelBuilder.Entity<BoothMenuItem>()
            .HasIndex(x => new { x.BoothId, x.IsDeleted });

        modelBuilder.Entity<PlaybackLog>()
            .HasIndex(x => new { x.BoothId, x.PlayedAtUtc });

        modelBuilder.Entity<PlaybackLog>()
            .HasIndex(x => new { x.VisitorUserId, x.PlayedAtUtc });

        modelBuilder.Entity<VisitorUser>()
            .HasIndex(x => x.VisitorCode)
            .IsUnique();

        modelBuilder.Entity<VisitorUser>()
            .HasIndex(x => x.DeviceKey);

        modelBuilder.Entity<BoothVisitLog>()
            .HasIndex(x => new { x.BoothId, x.VisitedAtUtc });

        modelBuilder.Entity<BoothVisitLog>()
            .HasIndex(x => new { x.VisitorUserId, x.VisitedAtUtc });
    }
}
