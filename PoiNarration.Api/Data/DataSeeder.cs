using Microsoft.EntityFrameworkCore;
using PoiNarration.Api.Models.Entities;

namespace PoiNarration.Api.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await db.Database.EnsureCreatedAsync();

        if (!await db.AppUsers.AnyAsync())
        {
            db.AppUsers.AddRange(
                new AppUser { Username = "admin", PasswordHash = "123456", Role = "Admin" },
                new AppUser { Username = "owner1", PasswordHash = "123456", Role = "Owner" },
                new AppUser { Username = "owner2", PasswordHash = "123456", Role = "Owner" }
            );

            await db.SaveChangesAsync();
        }
    }
}