using PoiNarration.Core.Models;

namespace PoiNarration.Mobile.Services;

public class GeofenceService
{
    private readonly AppDatabase _db;

    private string? _lastBoothId;
    private DateTime _lastPlayedUtc = DateTime.MinValue;

    public TimeSpan Cooldown { get; set; } = TimeSpan.FromMinutes(2);

    public GeofenceService(AppDatabase db)
    {
        _db = db;
    }

    public async Task<Booth?> CheckAndGetTriggeredBoothAsync(double userLat, double userLng)
    {
        return await FindTriggeredBoothAsync(userLat, userLng);
    }

    public async Task<Booth?> FindTriggeredBoothAsync(double userLat, double userLng)
    {
        var booths = await _db.GetAllBoothsAsync();

        var candidates = booths
            .Where(b => b.IsActive)
            .Select(b => new
            {
                Booth = b,
                Distance = CalculateDistanceMeters(userLat, userLng, b.Lat, b.Lng)
            })
            .Where(x => x.Distance <= x.Booth.RadiusMeters)
            .OrderByDescending(x => x.Booth.Priority)
            .ThenBy(x => x.Distance)
            .ToList();

        var selected = candidates.FirstOrDefault()?.Booth;
        if (selected == null) return null;

        if (_lastBoothId == selected.Id &&
            DateTime.UtcNow - _lastPlayedUtc < Cooldown)
        {
            return null;
        }

        return selected;
    }

    public void MarkPlayed(string boothId)
    {
        _lastBoothId = boothId;
        _lastPlayedUtc = DateTime.UtcNow;
    }

    private static double CalculateDistanceMeters(double lat1, double lng1, double lat2, double lng2)
    {
        const double R = 6371000;
        double dLat = DegreesToRadians(lat2 - lat1);
        double dLng = DegreesToRadians(lng2 - lng1);

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                   Math.Sin(dLng / 2) * Math.Sin(dLng / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }

    private static double DegreesToRadians(double deg) => deg * Math.PI / 180;
}