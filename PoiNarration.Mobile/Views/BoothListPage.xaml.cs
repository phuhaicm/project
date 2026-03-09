namespace PoiNarration.Mobile.Views;

public partial class BoothListPage : ContentPage
{
    public BoothListPage()
    {
        InitializeComponent();
    }

    private async void OnScanGateClicked(object sender, EventArgs e)
    {
        
        await Shell.Current.GoToAsync(nameof(GateModePage));
    }

    private async void OnManualClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ZoneListPage));
    }

    private async void OnOpenDetailClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(BoothDetailPage));
    }
}