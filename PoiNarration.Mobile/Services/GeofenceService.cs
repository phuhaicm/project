using PoiNarration.Core.Models;
using PoiNarration.Core.Utils;

namespace PoiNarration.Mobile.Services;

public class GeofenceService
{
    private readonly AppDatabase _db;

    // lưu thời điểm bắt đầu vào vùng
    private readonly Dictionary<string, DateTime> _enteredAt = new();

    // booth nào đã trigger trong lần đứng hiện tại
    private readonly HashSet<string> _triggeredInside = new();

    public GeofenceService(AppDatabase db)
    {
        _db = db;
    }

    public int DebounceSeconds { get; set; } = 1;   // debug trước, sau này tăng lên 3
    public int CooldownMinutes { get; set; } = 0;   // debug trước, sau này tăng lên 5

    public async Task<Booth?> CheckAndGetTriggeredBoothAsync(Location currentLocation, List<Booth> booths)
    {
        if (booths.Count == 0) return null;

        var insideBooths = booths
            .Select(b => new
            {
                Booth = b,
                Distance = GeoUtils.DistanceInMeters(
                    currentLocation.Latitude,
                    currentLocation.Longitude,
                    b.Lat,
                    b.Lng)
            })
            .Where(x => x.Distance <= x.Booth.RadiusMeters)
            .OrderByDescending(x => x.Booth.Priority)
            .ThenBy(x => x.Distance)
            .ToList();

        // booth đang nằm trong vùng hiện tại
        var insideIds = insideBooths.Select(x => x.Booth.Id).ToHashSet();

        // những booth đã rời khỏi vùng -> xóa trạng thái vào vùng
        var oldEnteredIds = _enteredAt.Keys.ToList();
        foreach (var id in oldEnteredIds)
        {
            if (!insideIds.Contains(id))
                _enteredAt.Remove(id);
        }

        // những booth đã rời khỏi vùng -> cho phép trigger lại khi vào lại
        var oldTriggeredIds = _triggeredInside.ToList();
        foreach (var id in oldTriggeredIds)
        {
            if (!insideIds.Contains(id))
                _triggeredInside.Remove(id);
        }

        // không có booth nào trong vùng
        if (insideBooths.Count == 0)
            return null;

        // chọn booth ưu tiên nhất / gần nhất
        var selected = insideBooths.First().Booth;

        // nếu vừa mới vào vùng lần đầu
        if (!_enteredAt.ContainsKey(selected.Id))
        {
            _enteredAt[selected.Id] = DateTime.UtcNow;
            return null;
        }

        // debounce
        var enteredAt = _enteredAt[selected.Id];
        var secondsInside = (DateTime.UtcNow - enteredAt).TotalSeconds;
        if (secondsInside < DebounceSeconds)
            return null;

        // nếu đã trigger trong lần đứng hiện tại thì không trigger lại
        if (_triggeredInside.Contains(selected.Id))
            return null;

        // cooldown
        var latestLog = await _db.GetLatestLogByBoothAsync(selected.Id);
        if (latestLog != null && CooldownMinutes > 0)
        {
            var diff = DateTime.UtcNow - latestLog.PlayedAtUtc;
            if (diff.TotalMinutes < CooldownMinutes)
                return null;
        }

        _triggeredInside.Add(selected.Id);
        return selected;
    }
}