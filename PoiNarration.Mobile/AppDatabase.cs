using SQLite;
using PoiNarration.Core.Models;

namespace PoiNarration.Mobile;

public class AppDatabase
{
    private readonly SQLiteAsyncConnection _db;

    public AppDatabase(string dbPath)
    {
        _db = new SQLiteAsyncConnection(dbPath);
    }

    public async Task InitAsync()
    {
        //if (_isInitialized) return;

        await _db.CreateTableAsync<Zone>();
        await _db.CreateTableAsync<Booth>();
        await _db.CreateTableAsync<BoothMenuItem>();
        await _db.CreateTableAsync<PlaybackLog>();

        //_isInitialized = true;
    }


    public Task<int> CountZonesAsync() => _db.Table<Zone>().CountAsync();

    public Task<List<Zone>> GetZonesAsync() =>
        _db.Table<Zone>().ToListAsync();

    public Task<List<Booth>> GetBoothsByZoneAsync(string zoneId) =>
        _db.Table<Booth>().Where(b => b.ZoneId == zoneId).ToListAsync();

    public async Task<Booth?> GetBoothAsync(string boothId)
    {
        var booth = await _db.Table<Booth>()
                             .Where(b => b.Id == boothId)
                             .FirstOrDefaultAsync();
        return booth;
    }

    //public Task<List<BoothMenuItem>> GetMenuByBoothAsync(string boothId) =>
        //_db.Table<BoothMenuItem>().Where(m => m.BoothId == boothId).ToListAsync();
    public async Task<List<BoothMenuItem>> GetMenuByBoothAsync(string boothId)
    {
        await InitAsync();

        return await _db.Table<BoothMenuItem>()
            .Where(x => x.BoothId == boothId && !x.IsDeleted)
            .ToListAsync();
    }

    public Task InsertAllAsync<T>(IEnumerable<T> items) where T : new() =>
        _db.InsertAllAsync(items);
    public Task<List<Booth>> GetAllBoothsAsync() =>
    _db.Table<Booth>().ToListAsync();
    public Task<int> InsertPlaybackLogAsync(PlaybackLog log) =>
    _db.InsertAsync(log);

    public Task<List<PlaybackLog>> GetLogsAsync() =>
        _db.Table<PlaybackLog>()
           .OrderByDescending(x => x.PlayedAtUtc)
           .ToListAsync();

    public async Task<PlaybackLog?> GetLatestLogByBoothAsync(string boothId)
    {
        var log = await _db.Table<PlaybackLog>()
            .Where(x => x.BoothId == boothId)
            .OrderByDescending(x => x.PlayedAtUtc)
            .FirstOrDefaultAsync();

        return log;
    }
    public async Task<int> UpsertBoothAsync(Booth booth)
    {
        await InitAsync();

        var existing = await _db.Table<Booth>()
            .Where(x => x.Id == booth.Id)
            .FirstOrDefaultAsync();

        if (existing == null)
            return await _db.InsertAsync(booth);

        return await _db.UpdateAsync(booth);
    }
    public async Task<int> UpsertMenuItemAsync(BoothMenuItem item)
    {
        await InitAsync();

        var existing = await _db.Table<BoothMenuItem>()
            .Where(x => x.Id == item.Id)
            .FirstOrDefaultAsync();

        if (existing == null)
            return await _db.InsertAsync(item);

        return await _db.UpdateAsync(item);
    }

}
