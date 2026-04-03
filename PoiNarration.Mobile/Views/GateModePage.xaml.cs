namespace PoiNarration.Mobile.Views;

public partial class GateModePage : ContentPage
{
    public GateModePage()
    {
        InitializeComponent();
    }

    private async void OnGpsModeClicked(object sender, EventArgs e)
    {
        
        await Shell.Current.GoToAsync("//map");
        
    }

    private async void OnManualModeClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ZoneListPage));
    }
    private async void OnBoothQrClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(QRScanPage));
    }

}
