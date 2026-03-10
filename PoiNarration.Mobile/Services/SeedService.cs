using System.Text.Json;
using PoiNarration.Core.Models;

namespace PoiNarration.Mobile.Services;

public class SeedData
{
    public List<Zone> Zones { get; set; } = new();
    public List<Booth> Booths { get; set; } = new();
    public List<BoothMenuItem> MenuItems { get; set; } = new();
}

public class SeedService
{
    private readonly AppDatabase _db;

    public SeedService(AppDatabase db)
    {
        _db = db;
    }

    public async Task EnsureSeededAsync()
    {
        await _db.InitAsync();

        var count = await _db.CountZonesAsync();
        if (count > 0) return;

        using var stream = await FileSystem.OpenAppPackageFileAsync("seed.json");
        using var reader = new StreamReader(stream);
        var json = await reader.ReadToEndAsync();

        var seed = JsonSerializer.Deserialize<SeedData>(
            json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        if (seed == null) return;

        await _db.InsertAllAsync(seed.Zones);
        await _db.InsertAllAsync(seed.Booths);
        await _db.InsertAllAsync(seed.MenuItems);
    }
}