using PoiNarration.Core.Models;

namespace PoiNarration.Mobile.Services;

public class GeofenceService
{
    private readonly AppDatabase _db;

    private string? _activeBoothId;
    private string? _candidateBoothId;
    private DateTime _candidateSinceUtc = DateTime.MinValue;

    private readonly Dictionary<string, DateTime> _lastTriggeredUtc = new();

    private readonly TimeSpan _dwellTime = TimeSpan.FromSeconds(2);
    private readonly TimeSpan _sameBoothCooldown = TimeSpan.FromSeconds(10);

    private const double MinEnterRadiusMeters = 50;
    private const double ExitBufferMeters = 15;
    private const double CandidateSwitchMarginMeters = 2;
    private const double ActiveSwitchMarginMeters = 2;

    public GeofenceService(AppDatabase db)
    {
        _db = db;
    }

    public async Task<GeofenceCheckResult> EvaluateAsync(double userLat, double userLng)
    {
        var booths = await _db.GetAllBoothsAsync();

        if (!booths.Any())
            return new GeofenceCheckResult();

        var distances = booths
            .Where(x => x.IsActive)
            .Select(x => new BoothDistance
            {
                Booth = x,
                DistanceMeters = CalculateDistanceMeters(userLat, userLng, x.Lat, x.Lng),
                EnterRadius = Math.Max(x.RadiusMeters, MinEnterRadiusMeters),
                ExitRadius = Math.Max(x.RadiusMeters, MinEnterRadiusMeters) + ExitBufferMeters
            })
            .OrderBy(x => x.DistanceMeters)
            .ThenBy(x => x.Booth.Priority)
            .ToList();

        var nearestAny = distances.FirstOrDefault();

        var result = new GeofenceCheckResult
        {
            NearestBooth = nearestAny?.Booth,
            NearestDistanceMeters = nearestAny?.DistanceMeters ?? double.MaxValue,
            ActiveBoothId = _activeBoothId
        };

        var insideCandidates = distances
            .Where(x => x.DistanceMeters <= x.EnterRadius)
            .OrderBy(x => x.DistanceMeters)
            .ThenBy(x => x.Booth.Priority)
            .ToList();

        if (!insideCandidates.Any())
        {
            if (!string.IsNullOrWhiteSpace(_activeBoothId))
            {
                var activeDistance = distances.FirstOrDefault(x => x.Booth.Id == _activeBoothId);
                if (activeDistance == null || activeDistance.DistanceMeters > activeDistance.ExitRadius)
                {
                    _activeBoothId = null;
                }
            }

            _candidateBoothId = null;
            _candidateSinceUtc = DateTime.MinValue;
            result.ActiveBoothId = _activeBoothId;
            return result;
        }

        var nearestInside = insideCandidates.First();

        // Ổn định candidate để tránh jitter khi nhiều booth gần nhau
        if (string.IsNullOrWhiteSpace(_candidateBoothId))
        {
            _candidateBoothId = nearestInside.Booth.Id;
            _candidateSinceUtc = DateTime.UtcNow;
            return result;
        }

        var currentCandidate = insideCandidates.FirstOrDefault(x => x.Booth.Id == _candidateBoothId);

        if (currentCandidate == null)
        {
            _candidateBoothId = nearestInside.Booth.Id;
            _candidateSinceUtc = DateTime.UtcNow;
            return result;
        }

        if (nearestInside.Booth.Id != _candidateBoothId)
        {
            if (nearestInside.DistanceMeters + CandidateSwitchMarginMeters < currentCandidate.DistanceMeters)
            {
                _candidateBoothId = nearestInside.Booth.Id;
                _candidateSinceUtc = DateTime.UtcNow;
            }

            return result;
        }

        // Candidate đã ổn định, chờ đủ dwell 2 giây
        if (DateTime.UtcNow - _candidateSinceUtc < _dwellTime)
        {
            return result;
        }

        bool shouldSwitch = false;

        if (string.IsNullOrWhiteSpace(_activeBoothId))
        {
            shouldSwitch = true;
        }
        else if (_activeBoothId == _candidateBoothId)
        {
            shouldSwitch = false;
        }
        else
        {
            var activeDistance = distances.FirstOrDefault(x => x.Booth.Id == _activeBoothId);
            var candidateDistance = distances.FirstOrDefault(x => x.Booth.Id == _candidateBoothId);

            if (candidateDistance == null)
            {
                shouldSwitch = false;
            }
            else if (activeDistance == null)
            {
                shouldSwitch = true;
            }
            else if (activeDistance.DistanceMeters > activeDistance.ExitRadius)
            {
                shouldSwitch = true;
            }
            else if (candidateDistance.DistanceMeters + ActiveSwitchMarginMeters < activeDistance.DistanceMeters)
            {
                shouldSwitch = true;
            }
        }

        if (shouldSwitch && CanTrigger(_candidateBoothId))
        {
            var booth = distances.First(x => x.Booth.Id == _candidateBoothId).Booth;

            _activeBoothId = booth.Id;
            _lastTriggeredUtc[booth.Id] = DateTime.UtcNow;

            result.ActiveBoothId = _activeBoothId;
            result.ShouldTrigger = true;
            result.TriggeredBooth = booth;
        }

        return result;
    }

    public void Reset()
    {
        _activeBoothId = null;
        _candidateBoothId = null;
        _candidateSinceUtc = DateTime.MinValue;
    }

    private bool CanTrigger(string? boothId)
    {
        if (string.IsNullOrWhiteSpace(boothId))
            return false;

        if (_lastTriggeredUtc.TryGetValue(boothId, out var lastTime))
        {
            if (DateTime.UtcNow - lastTime < _sameBoothCooldown)
                return false;
        }

        return true;
    }

    private static double CalculateDistanceMeters(double lat1, double lng1, double lat2, double lng2)
    {
        const double R = 6371000;

        double dLat = DegreesToRadians(lat2 - lat1);
        double dLng = DegreesToRadians(lng2 - lng1);

        double a =
            Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
            Math.Sin(dLng / 2) * Math.Sin(dLng / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }

    private static double DegreesToRadians(double deg)
    {
        return deg * Math.PI / 180.0;
    }

    private class BoothDistance
    {
        public Booth Booth { get; set; } = null!;
        public double DistanceMeters { get; set; }
        public double EnterRadius { get; set; }
        public double ExitRadius { get; set; }
    }
}

public class GeofenceCheckResult
{
    public Booth? NearestBooth { get; set; }
    public double NearestDistanceMeters { get; set; }

    public string? ActiveBoothId { get; set; }

    public bool ShouldTrigger { get; set; }
    public Booth? TriggeredBooth { get; set; }
}