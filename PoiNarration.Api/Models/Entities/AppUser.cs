namespace PoiNarration.Api.Models.Entities;

public class AppUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Username { get; set; } = "";

    // THÊM DÒNG NÀY ĐỂ HẾT LỖI GẠCH ĐỎ
    public string PasswordHash { get; set; } = "";

    public string Password { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Role { get; set; } = "Owner";
}