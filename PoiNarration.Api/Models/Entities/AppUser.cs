namespace PoiNarration.Api.Models.Entities;

public class AppUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Username { get; set; } = "";
    public string PasswordHash { get; set; } = "";

    // Admin / Owner
    public string Role { get; set; } = "Owner";
}