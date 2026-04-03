namespace PoiNarration.Mobile.Services;

public class SyncService
{
    private readonly ApiService _api;
    private readonly AppDatabase _db;

    public SyncService(ApiService api, AppDatabase db)
    {
        _api = api;
        _db = db;
    }

    public async Task SyncAsync()
    {
        var payload = await _api.GetBootstrapAsync();
        if (payload == null) return;

        foreach (var booth in payload.Booths)
        {
            await _db.UpsertBoothAsync(booth);
        }

        foreach (var menu in payload.MenuItems)
        {
            await _db.UpsertMenuItemAsync(menu);
        }
    }
}