using PoiNarration.Core.Models;
using SQLite;

namespace PoiNarration.Mobile;

public class AppDatabase
{
    private readonly string _databasePath;
    private SQLiteAsyncConnection? _database;

    // ===== Constructor khớp với MauiProgram.cs =====
    public AppDatabase(string databasePath)
    {
        _databasePath = databasePath;
    }

    public async Task InitAsync()
    {
        if (_database != null)
            return;

        _database = new SQLiteAsyncConnection(_databasePath, Constants.Flags);

        // ===== Bảng cũ tuần 5 =====
        await _database.CreateTableAsync<Zone>();
        await _database.CreateTableAsync<Booth>();
        await _database.CreateTableAsync<BoothMenuItem>();

        // ===== Bảng mới cho giai đoạn B =====
        await _database.CreateTableAsync<PlaybackLogLocal>();
        await _database.CreateTableAsync<BoothTranslationLocal>();
        await _database.CreateTableAsync<BoothMenuItemTranslationLocal>();
    }

    // =====================================================
    // ZONE
    // =====================================================
    public async Task<int> CountZonesAsync()
    {
        await InitAsync();
        return await _database!.Table<Zone>().CountAsync();
    }

    public async Task<List<Zone>> GetZonesAsync()
    {
        await InitAsync();
        return await _database!.Table<Zone>().ToListAsync();
    }

    // =====================================================
    // GENERIC INSERT ALL (phục vụ SeedService tuần 5)
    // =====================================================
    public async Task<int> InsertAllAsync<T>(IEnumerable<T> items) where T : new()
    {
        await InitAsync();
        return await _database!.InsertAllAsync(items);
    }

    // =====================================================
    // BOOTH
    // =====================================================
    public async Task<int> UpsertBoothAsync(Booth booth)
    {
        await InitAsync();
        return await _database!.InsertOrReplaceAsync(booth);
    }

    public async Task<List<Booth>> GetAllBoothsAsync()
    {
        await InitAsync();
        return await _database!.Table<Booth>().ToListAsync();
    }

    public async Task<Booth?> GetBoothAsync(string boothId)
    {
        await InitAsync();
        return await _database!.Table<Booth>()
            .FirstOrDefaultAsync(x => x.Id == boothId);
    }

    public async Task<List<Booth>> GetBoothsByZoneAsync(string zoneId)
    {
        await InitAsync();
        return await _database!.Table<Booth>()
            .Where(x => x.ZoneId == zoneId)
            .ToListAsync();
    }

    // =====================================================
    // MENU
    // =====================================================
    public async Task<int> UpsertMenuItemAsync(BoothMenuItem item)
    {
        await InitAsync();
        return await _database!.InsertOrReplaceAsync(item);
    }

    public async Task<List<BoothMenuItem>> GetMenuByBoothAsync(string boothId)
    {
        await InitAsync();
        return await _database!.Table<BoothMenuItem>()
            .Where(x => x.BoothId == boothId && !x.IsDeleted)
            .ToListAsync();
    }

    // =====================================================
    // PLAYBACK LOG LOCAL (giai đoạn B)
    // =====================================================
    public async Task<int> SavePlaybackLogAsync(PlaybackLogLocal item)
    {
        await InitAsync();
        return await _database!.InsertAsync(item);
    }

    public async Task<List<PlaybackLogLocal>> GetUnsyncedPlaybackLogsAsync()
    {
        await InitAsync();
        return await _database!.Table<PlaybackLogLocal>()
            .Where(x => !x.IsSynced)
            .ToListAsync();
    }

    public async Task<int> MarkPlaybackLogSyncedAsync(int id)
    {
        await InitAsync();

        var log = await _database!.Table<PlaybackLogLocal>()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (log == null)
            return 0;

        log.IsSynced = true;
        return await _database.UpdateAsync(log);
    }
    public async Task<int> UpsertBoothTranslationAsync(BoothTranslationLocal item)
    {
        await InitAsync();
        return await _database!.InsertOrReplaceAsync(item);
    }

    public async Task<List<BoothTranslationLocal>> GetBoothTranslationsAsync(string boothId)
    {
        await InitAsync();
        return await _database!.Table<BoothTranslationLocal>()
            .Where(x => x.BoothId == boothId)
            .ToListAsync();
    }

    public async Task<BoothTranslationLocal?> GetBoothTranslationAsync(string boothId, string lang)
    {
        await InitAsync();
        return await _database!.Table<BoothTranslationLocal>()
            .FirstOrDefaultAsync(x => x.BoothId == boothId && x.LanguageCode == lang);
    }
    public async Task<int> UpsertMenuTranslationAsync(BoothMenuItemTranslationLocal item)
    {
        await InitAsync();
        return await _database!.InsertOrReplaceAsync(item);
    }

    public async Task<List<BoothMenuItemTranslationLocal>> GetMenuTranslationsAsync(string menuItemId)
    {
        await InitAsync();
        return await _database!.Table<BoothMenuItemTranslationLocal>()
            .Where(x => x.MenuItemId == menuItemId)
            .ToListAsync();
    }
    public async Task<int> UpsertZoneAsync(Zone zone)
    {
        await InitAsync();
        return await _database!.InsertOrReplaceAsync(zone);
    }

}
