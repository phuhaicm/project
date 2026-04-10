namespace PoiNarration.Api.Models.Entities;

public class TourBooth
{
    public int Id { get; set; }
    public int TourId { get; set; }
    public string BoothId { get; set; } = "";
    public int Order { get; set; }
}
