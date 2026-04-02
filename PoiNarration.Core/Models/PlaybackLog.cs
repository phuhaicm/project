public class PlaybackLog
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string BoothId { get; set; } = "";

    public string TriggerType { get; set; } = ""; // GPS / QR / Manual
    public string Language { get; set; } = "vi";

    public DateTime PlayedAtUtc { get; set; } = DateTime.UtcNow;

    public double Lat { get; set; }
    public double Lng { get; set; }

    public bool IsCompleted { get; set; } = true;
}
