namespace PoiNarration.Mobile.Views;

public partial class ZoneListPage : ContentPage
{
    public ZoneListPage()
    {
        InitializeComponent();
    }

    private async void OnZoneAClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(BoothByZonePage)}?zoneId=zone-a");
    }

    private async void OnZoneBClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(BoothByZonePage)}?zoneId=zone-b");
    }
}