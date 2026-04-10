using SQLite; // Quan trọng để App Mobile chạy được

namespace PoiNarration.Core.Models;

public class Tour
{
    [PrimaryKey, AutoIncrement] // SQLite cần cái này để tự quản lý ID
    public int Id { get; set; }

    public string Name { get; set; } = "";
    public string Description { get; set; } = "";

    // Bạn có thể thêm các trường này nếu cần thiết cho Mobile
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
}