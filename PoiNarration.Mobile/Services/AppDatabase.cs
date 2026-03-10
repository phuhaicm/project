using SQLite;
using PoiNarration.Core.Models;

namespace PoiNarration.Mobile.Services;

public class AppDatabase
{
    private readonly SQLiteAsyncConnection _db;

    public AppDatabase(string dbPath)
    {
        _db = new SQLiteAsyncConnection(dbPath);
    }

    public async Task InitAsync()
    {
        await _db.CreateTableAsync<Zone>();
        await _db.CreateTableAsync<Booth>();
        await _db.CreateTableAsync<BoothMenuItem>();
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

    public Task<List<BoothMenuItem>> GetMenuByBoothAsync(string boothId) =>
        _db.Table<BoothMenuItem>().Where(m => m.BoothId == boothId).ToListAsync();

    public Task InsertAllAsync<T>(IEnumerable<T> items) where T : new() =>
        _db.InsertAllAsync(items);
    public Task<List<Booth>> GetAllBoothsAsync() =>
    _db.Table<Booth>().ToListAsync();
}
