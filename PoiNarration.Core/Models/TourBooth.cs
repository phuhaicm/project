using SQLite;

namespace PoiNarration.Core.Models;

public class TourBooth
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int TourId { get; set; }

    // Lưu ý: BoothId ở đây phải là string để khớp với kiểu Id (Guid) của Booth
    public string BoothId { get; set; } = "";

    public int Order { get; set; }
}