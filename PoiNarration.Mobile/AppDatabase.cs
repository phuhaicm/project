using System.Diagnostics;
using System.IO;
using PoiNarration.Core.Models;
using SQLite;


namespace PoiNarration.Mobile;

public class AppDatabase
{
    private readonly string _databasePath;
    private SQLiteAsyncConnection? _database;
    private readonly SemaphoreSlim _initLock = new(1, 1);

    // ===== Constructor khớp với MauiProgram.cs =====
    public AppDatabase(string databasePath)
    {
        _databasePath = databasePath;
    }

    public async Task InitAsync()
    {
        await _initLock.WaitAsync();

        try
        {
            if (_database != null)
            {
                await EnsureSchemaAsync(_database);
                return;
            }

            var conn = new SQLiteAsyncConnection(_databasePath, Constants.Flags);

            try
            {
                // Tạo bảng chính trước
                await conn.CreateTableAsync<Booth>();
                await conn.CreateTableAsync<BoothMenuItem>();
                await conn.CreateTableAsync<PlaybackLog>();
                await conn.CreateTableAsync<BoothTranslationLocal>();
                await conn.CreateTableAsync<BoothMenuItemTranslationLocal>();
                await conn.CreateTableAsync<Zone>();

                await EnsureSchemaAsync(conn);
                await EnsureMenuTranslationSchemaAsync(conn);


                _database = conn;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[InitAsync lỗi khi tạo bảng]: {ex}");

                try
                {
                    await conn.CloseAsync();
                }
                catch
                {
                    // ignore
                }

                if (File.Exists(_databasePath))
                {
                    try
                    {
                        File.Delete(_databasePath);
                    }
                    catch
                    {
                        // ignore
                    }
                }

                _database = null;
                throw;
            }
        }
        finally
        {
            _initLock.Release();
        }
    }
    private static async Task EnsureSchemaAsync(SQLiteAsyncConnection conn)
    {
        var boothTableExists = await conn.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='Booth'");

        if (boothTableExists == 0)
            throw new Exception("Bảng Booth chưa được tạo trong SQLite.");
    }

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
    public async Task<int> UpsertBoothAsync(Booth booth)
    {
        await InitAsync();
        return await _database!.InsertOrReplaceAsync(booth);
    }

    public async Task<int> InsertAllAsync<T>(IEnumerable<T> items) where T : new()
    {
        await InitAsync();
        return await _database!.InsertAllAsync(items);
    }



    public async Task<List<Booth>> GetAllBoothsAsync()
    {
        await InitAsync();
        // Đã kẹp thêm điều kiện lọc: Chỉ lấy những gian hàng có IsActive là true
        return await _database!.Table<Booth>().Where(x => x.IsActive).ToListAsync();
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
            .Where(x => x.ZoneId == zoneId && x.IsActive) // <-- Thêm && x.IsActive ở đây
            .ToListAsync();
    }

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

    public async Task<int> SavePlaybackLogAsync(PlaybackLog item)
    {
        await InitAsync();
        return await _database!.InsertAsync(item);
    }

    public async Task<List<PlaybackLog>> GetUnsyncedPlaybackLogsAsync()
    {
        await InitAsync();
        return await _database!.Table<PlaybackLog>()
            .Where(x => !x.IsSynced)
            .ToListAsync();
    }

    public async Task<int> MarkPlaybackLogSyncedAsync(int id)
    {
        await InitAsync();

        var log = await _database!.Table<PlaybackLog>()
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
    // Thêm hàm này vào cuối class AppDatabase

    public async Task SaveBootstrapDataAsync(BootstrapSyncResponse data)
    {
        await InitAsync();
        // 1. CHẶN ĐƯỜNG VÀ ĐỔI LINK ẢNH TRƯỚC KHI LƯU VÀO SQLITE
        // Lấy BaseUrl xịn (Tự động biết là 10.0.2.2 hay 192.168...)
        string baseUrl = PoiNarration.Mobile.Services.ApiConstants.GetBaseUrl();

        // 1. "Rửa" link ảnh cho Booth (NÂNG CẤP)
        if (data.Booths != null)
        {
            foreach (var booth in data.Booths)
            {
                // Chỉ cần link chứa chữ "/uploads/" là tự động thay thế IP mới nhất vào
                if (!string.IsNullOrEmpty(booth.ImageUrl) && booth.ImageUrl.Contains("/uploads/"))
                {
                    var uri = new Uri(booth.ImageUrl);
                    booth.ImageUrl = $"{baseUrl.TrimEnd('/')}{uri.PathAndQuery}";
                }
            }
        }

        // 2. "Rửa" link ảnh cho Menu Items (NÂNG CẤP)
        if (data.MenuItems != null)
        {
            foreach (var item in data.MenuItems)
            {
                if (!string.IsNullOrEmpty(item.ImageUrl) && item.ImageUrl.Contains("/uploads/"))
                {
                    var uri = new Uri(item.ImageUrl);
                    item.ImageUrl = $"{baseUrl.TrimEnd('/')}{uri.PathAndQuery}";
                }
            }
        }
        // Sử dụng RunInTransactionAsync để tất cả lệnh SQL chạy trong 1 phiên duy nhất
        await _database!.RunInTransactionAsync(conn =>
        {
            // 1. Xóa dữ liệu cũ (Tùy chọn: Nếu bạn muốn làm sạch DB mỗi lần sync)
            conn.DeleteAll<Booth>();
            conn.DeleteAll<BoothMenuItem>();
            conn.DeleteAll<BoothTranslationLocal>();
            conn.DeleteAll<BoothMenuItemTranslationLocal>();
            conn.DeleteAll<Zone>();

            // 2. Chèn dữ liệu mới hàng loạt (Cực kỳ nhanh)
            if (data.Zones?.Any() == true) conn.InsertAll(data.Zones);
            if (data.Booths?.Any() == true) conn.InsertAll(data.Booths);
            if (data.MenuItems?.Any() == true) conn.InsertAll(data.MenuItems);
            if (data.BoothTranslations?.Any() == true) conn.InsertAll(data.BoothTranslations);
            if (data.MenuTranslations?.Any() == true) conn.InsertAll(data.MenuTranslations);

            System.Diagnostics.Debug.WriteLine("Đã lưu Bootstrap vào SQLite thành công!");
        });
    }
    public async Task ClearSyncTablesAsync()
    {
        await InitAsync();


        await _database!.DeleteAllAsync<Zone>();
        await _database!.DeleteAllAsync<Booth>();
        await _database.DeleteAllAsync<BoothMenuItem>();
        await _database.DeleteAllAsync<BoothTranslationLocal>();
        await _database.DeleteAllAsync<BoothMenuItemTranslationLocal>();
    }
    private static async Task EnsureMenuTranslationSchemaAsync(SQLiteAsyncConnection conn)
    {
        var tableInfo = await conn.QueryAsync<TableInfoRow>(
            "PRAGMA table_info('BoothMenuItemTranslationLocal')");

        var columnNames = tableInfo.Select(x => x.name).ToHashSet(StringComparer.OrdinalIgnoreCase);

        if (!columnNames.Contains("CurrencyCode"))
        {
            await conn.ExecuteAsync(
                "ALTER TABLE BoothMenuItemTranslationLocal ADD COLUMN CurrencyCode TEXT NOT NULL DEFAULT 'VND'");
        }

        if (!columnNames.Contains("LocalizedPrice"))
        {
            await conn.ExecuteAsync(
                "ALTER TABLE BoothMenuItemTranslationLocal ADD COLUMN LocalizedPrice REAL NULL");
        }

        if (!columnNames.Contains("PriceText"))
        {
            await conn.ExecuteAsync(
                "ALTER TABLE BoothMenuItemTranslationLocal ADD COLUMN PriceText TEXT NULL");
        }
    }

    private class TableInfoRow
    {
        public int cid { get; set; }
        public string name { get; set; } = "";
        public string type { get; set; } = "";
        public int notnull { get; set; }
        public string? dflt_value { get; set; }
        public int pk { get; set; }
    }


}
