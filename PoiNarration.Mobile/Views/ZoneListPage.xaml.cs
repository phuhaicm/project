namespace PoiNarration.Mobile.Views;

public partial class ZoneListPage : ContentPage
{
    public ZoneListPage()
    {
        InitializeComponent();
    }

    private async void OnZoneClicked(object sender, EventArgs e)
    {
        // Tuần 1: chưa truyền zoneId, chỉ chuyển trang
        await Shell.Current.GoToAsync(nameof(BoothListPage));
    }
}
