using PoiNarration.Core.Models;

namespace PoiNarration.Mobile.Services;

public class GeofenceService
{
    private readonly AppDatabase _db;

    // Booth đã trigger gần nhất
    private string? _activeBoothId;

    // Booth gần nhất đang được theo dõi để chờ đủ dwell time
    private string? _candidateBoothId;
    private DateTime _candidateSinceUtc = DateTime.MinValue;

    // Đứng gần 2 giây là trigger
    private readonly TimeSpan _dwellTime = TimeSpan.FromSeconds(2);

    // Thông số tinh chỉnh cho emulator / test indoor
    private const double MinRadiusMeters = 60;      // tăng để dễ trigger hơn
    private const double SwitchMarginMeters = 1.0;  // booth mới chỉ cần gần hơn ~1m là được ưu tiên
    private const double ExitBufferMeters = 6;      // ra xa hơn bán kính cũ + 6m mới coi là rời booth

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
                RadiusMeters = Math.Max(x.RadiusMeters, MinRadiusMeters)
            })
            .OrderBy(x => x.DistanceMeters)
            .ThenBy(x => x.Booth.Priority)
            .ToList();

        var nearest = distances.FirstOrDefault();

        var result = new GeofenceCheckResult
        {
            NearestBooth = nearest?.Booth,
            NearestDistanceMeters = nearest?.DistanceMeters ?? double.MaxValue,
            ActiveBoothId = _activeBoothId
        };

        if (nearest == null)
            return result;

        // 1) Nếu nearest còn ở ngoài phạm vi -> chỉ update panel nearest booth, chưa trigger
        if (nearest.DistanceMeters > nearest.RadiusMeters)
        {
            // Nếu booth active cũ đã bị bỏ quá xa thì reset
            if (!string.IsNullOrWhiteSpace(_activeBoothId))
            {
                var active = distances.FirstOrDefault(x => x.Booth.Id == _activeBoothId);
                if (active == null || active.DistanceMeters > active.RadiusMeters + ExitBufferMeters)
                {
                    _activeBoothId = null;
                }
            }

            _candidateBoothId = null;
            _candidateSinceUtc = DateTime.MinValue;
            result.ActiveBoothId = _activeBoothId;
            return result;
        }

        // 2) Nearest booth đang trong phạm vi -> theo dõi candidate
        if (_candidateBoothId != nearest.Booth.Id)
        {
            _candidateBoothId = nearest.Booth.Id;
            _candidateSinceUtc = DateTime.UtcNow;
            return result;
        }

        // 3) Chưa đứng đủ gần 2 giây -> chưa trigger
        if (DateTime.UtcNow - _candidateSinceUtc < _dwellTime)
        {
            return result;
        }

        // 4) Chưa có active booth -> trigger nearest ngay
        if (string.IsNullOrWhiteSpace(_activeBoothId))
        {
            _activeBoothId = nearest.Booth.Id;
            result.ActiveBoothId = _activeBoothId;
            result.ShouldTrigger = true;
            result.TriggeredBooth = nearest.Booth;
            return result;
        }

        // 5) Booth active hiện tại chính là nearest -> không trigger lại
        if (_activeBoothId == nearest.Booth.Id)
        {
            result.ActiveBoothId = _activeBoothId;
            return result;
        }

        // 6) Xét có nên bỏ booth cũ để switch sang booth mới không
        var currentActive = distances.FirstOrDefault(x => x.Booth.Id == _activeBoothId);

        bool shouldSwitch = false;

        if (currentActive == null)
        {
            shouldSwitch = true;
        }
        else
        {
            // booth cũ đã bị bỏ ra ngoài vùng
            if (currentActive.DistanceMeters > currentActive.RadiusMeters + ExitBufferMeters)
            {
                shouldSwitch = true;
            }
            // booth mới gần hơn rõ ràng
            else if (nearest.DistanceMeters + SwitchMarginMeters < currentActive.DistanceMeters)
            {
                shouldSwitch = true;
            }
        }

        if (shouldSwitch)
        {
            _activeBoothId = nearest.Booth.Id;
            result.ActiveBoothId = _activeBoothId;
            result.ShouldTrigger = true;
            result.TriggeredBooth = nearest.Booth;
        }

        return result;
    }

    public void Reset()
    {
        _activeBoothId = null;
        _candidateBoothId = null;
        _candidateSinceUtc = DateTime.MinValue;
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
        public double RadiusMeters { get; set; }
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
