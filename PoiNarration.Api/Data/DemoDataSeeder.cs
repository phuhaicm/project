using Microsoft.EntityFrameworkCore;
using PoiNarration.Core.Models;

namespace PoiNarration.Api.Data;

public static class DemoDataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await db.Database.EnsureCreatedAsync();

        var now = DateTime.UtcNow;

        // 1) Seed VisitorUsers nếu đang trống
        if (!await db.VisitorUsers.AnyAsync())
        {
            var visitors = new List<VisitorUser>
{
    new VisitorUser
    {
        Id = "demo-visitor-001",
        VisitorCode = "VIS-000001",
        DisplayName = "Thiết bị demo 01",
        DeviceKey = "demo-device-001",
        PreferredLanguage = "vi",
        Platform = "Android",
        AppVersion = "1.0.0",
        CreatedAtUtc = now.AddDays(-5),
        LastActiveAtUtc = now.AddHours(-2),
        IsActive = false
    },
    new VisitorUser
    {
        Id = "demo-visitor-002",
        VisitorCode = "VIS-000002",
        DisplayName = "Thiết bị demo 02",
        DeviceKey = "demo-device-002",
        PreferredLanguage = "en",
        Platform = "Android",
        AppVersion = "1.0.0",
        CreatedAtUtc = now.AddDays(-4),
        LastActiveAtUtc = now.AddHours(-3),
        IsActive = false
    },
    new VisitorUser
    {
        Id = "demo-visitor-003",
        VisitorCode = "VIS-000003",
        DisplayName = "Thiết bị demo 03",
        DeviceKey = "demo-device-003",
        PreferredLanguage = "fr",
        Platform = "Android",
        AppVersion = "1.0.0",
        CreatedAtUtc = now.AddDays(-3),
        LastActiveAtUtc = now.AddHours(-4),
        IsActive = false
    },
    new VisitorUser
    {
        Id = "demo-visitor-004",
        VisitorCode = "VIS-000004",
        DisplayName = "Thiết bị demo 04",
        DeviceKey = "demo-device-004",
        PreferredLanguage = "vi",
        Platform = "Android",
        AppVersion = "1.0.0",
        CreatedAtUtc = now.AddDays(-2),
        LastActiveAtUtc = now.AddHours(-5),
        IsActive = false
    },
    new VisitorUser
    {
        Id = "demo-visitor-005",
        VisitorCode = "VIS-000005",
        DisplayName = "Thiết bị demo 05",
        DeviceKey = "demo-device-005",
        PreferredLanguage = "zh",
        Platform = "Android",
        AppVersion = "1.0.0",
        CreatedAtUtc = now.AddDays(-1),
        LastActiveAtUtc = now.AddHours(-6),
        IsActive = false
    }
};

            db.VisitorUsers.AddRange(visitors);
            await db.SaveChangesAsync();
        }

        // 2) Seed BoothVisitLogs nếu đang trống
        if (!await db.BoothVisitLogs.AnyAsync())
        {
            var visitLogs = new List<BoothVisitLog>
            {
                new BoothVisitLog
                {
                    VisitorUserId = "demo-visitor-001",
                    BoothId = "booth-01",
                    TriggerType = "ManualOpen",
                    Language = "vi",
                    VisitedAtUtc = now.AddHours(-6),
                    SessionId = "demo-session-001",
                    IsSynced = true
                },
                new BoothVisitLog
                {
                    VisitorUserId = "demo-visitor-001",
                    BoothId = "booth-03",
                    TriggerType = "QR",
                    Language = "vi",
                    VisitedAtUtc = now.AddHours(-5),
                    SessionId = "demo-session-001",
                    IsSynced = true
                },
                new BoothVisitLog
                {
                    VisitorUserId = "demo-visitor-002",
                    BoothId = "booth-01",
                    TriggerType = "ManualOpen",
                    Language = "en",
                    VisitedAtUtc = now.AddHours(-4),
                    SessionId = "demo-session-002",
                    IsSynced = true
                },
                new BoothVisitLog
                {
                    VisitorUserId = "demo-visitor-003",
                    BoothId = "booth-02",
                    TriggerType = "MapTap",
                    Language = "fr",
                    VisitedAtUtc = now.AddHours(-3),
                    SessionId = "demo-session-003",
                    IsSynced = true
                },
                new BoothVisitLog
                {
                    VisitorUserId = "demo-visitor-004",
                    BoothId = "booth-01",
                    TriggerType = "NearestButton",
                    Language = "vi",
                    VisitedAtUtc = now.AddHours(-2),
                    SessionId = "demo-session-004",
                    IsSynced = true
                },
                new BoothVisitLog
                {
                    VisitorUserId = "demo-visitor-004",
                    BoothId = "booth-02",
                    TriggerType = "ManualOpen",
                    Language = "vi",
                    VisitedAtUtc = now.AddHours(-2).AddMinutes(10),
                    SessionId = "demo-session-004",
                    IsSynced = true
                },
                new BoothVisitLog
                {
                    VisitorUserId = "demo-visitor-004",
                    BoothId = "booth-04",
                    TriggerType = "QR",
                    Language = "vi",
                    VisitedAtUtc = now.AddHours(-2).AddMinutes(20),
                    SessionId = "demo-session-004",
                    IsSynced = true
                },
                new BoothVisitLog
                {
                    VisitorUserId = "demo-visitor-005",
                    BoothId = "booth-04",
                    TriggerType = "ManualOpen",
                    Language = "zh",
                    VisitedAtUtc = now.AddHours(-1),
                    SessionId = "demo-session-005",
                    IsSynced = true
                }
            };

            db.BoothVisitLogs.AddRange(visitLogs);
            await db.SaveChangesAsync();
        }

        // 3) Seed PlaybackLogs nếu đang trống
        if (!await db.PlaybackLogs.AnyAsync())
        {
            var playbackLogs = new List<PlaybackLog>
            {
                new PlaybackLog
                {
                    VisitorUserId = "demo-visitor-001",
                    BoothId = "booth-01",
                    TriggerType = "Manual",
                    Language = "vi",
                    PlayedAtUtc = now.AddHours(-5),
                    DurationSeconds = 12,
                    IsCompleted = true,
                    SessionId = "demo-session-001",
                    IsSynced = true
                },
                new PlaybackLog
                {
                    VisitorUserId = "demo-visitor-001",
                    BoothId = "booth-01",
                    TriggerType = "Manual",
                    Language = "vi",
                    PlayedAtUtc = now.AddHours(-4),
                    DurationSeconds = 14,
                    IsCompleted = true,
                    SessionId = "demo-session-001",
                    IsSynced = true
                },
                new PlaybackLog
                {
                    VisitorUserId = "demo-visitor-002",
                    BoothId = "booth-01",
                    TriggerType = "Manual",
                    Language = "en",
                    PlayedAtUtc = now.AddHours(-3),
                    DurationSeconds = 10,
                    IsCompleted = true,
                    SessionId = "demo-session-002",
                    IsSynced = true
                },
                new PlaybackLog
                {
                    VisitorUserId = "demo-visitor-003",
                    BoothId = "booth-02",
                    TriggerType = "Manual",
                    Language = "fr",
                    PlayedAtUtc = now.AddHours(-2),
                    DurationSeconds = 9,
                    IsCompleted = true,
                    SessionId = "demo-session-003",
                    IsSynced = true
                },
                new PlaybackLog
                {
                    VisitorUserId = "demo-visitor-003",
                    BoothId = "booth-02",
                    TriggerType = "Manual",
                    Language = "fr",
                    PlayedAtUtc = now.AddHours(-2).AddMinutes(5),
                    DurationSeconds = 11,
                    IsCompleted = true,
                    SessionId = "demo-session-003",
                    IsSynced = true
                },
                new PlaybackLog
                {
                    VisitorUserId = "demo-visitor-003",
                    BoothId = "booth-02",
                    TriggerType = "Manual",
                    Language = "fr",
                    PlayedAtUtc = now.AddHours(-2).AddMinutes(10),
                    DurationSeconds = 13,
                    IsCompleted = true,
                    SessionId = "demo-session-003",
                    IsSynced = true
                },
                new PlaybackLog
                {
                    VisitorUserId = "demo-visitor-004",
                    BoothId = "booth-04",
                    TriggerType = "Manual",
                    Language = "vi",
                    PlayedAtUtc = now.AddHours(-1),
                    DurationSeconds = 8,
                    IsCompleted = true,
                    SessionId = "demo-session-004",
                    IsSynced = true
                },
                new PlaybackLog
                {
                    VisitorUserId = "demo-visitor-005",
                    BoothId = "booth-03",
                    TriggerType = "Manual",
                    Language = "zh",
                    PlayedAtUtc = now.AddMinutes(-45),
                    DurationSeconds = 15,
                    IsCompleted = true,
                    SessionId = "demo-session-005",
                    IsSynced = true
                },
                new PlaybackLog
                {
                    VisitorUserId = "demo-visitor-005",
                    BoothId = "booth-03",
                    TriggerType = "Manual",
                    Language = "zh",
                    PlayedAtUtc = now.AddMinutes(-40),
                    DurationSeconds = 12,
                    IsCompleted = true,
                    SessionId = "demo-session-005",
                    IsSynced = true
                }
            };

            db.PlaybackLogs.AddRange(playbackLogs);
            await db.SaveChangesAsync();
        }
    }
}