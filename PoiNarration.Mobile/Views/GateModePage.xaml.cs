namespace PoiNarration.Mobile.Views;

public partial class GateModePage : ContentPage
{
    public GateModePage()
    {
        InitializeComponent();
    }

    private async void OnGpsModeClicked(object sender, EventArgs e)
    {
        // Tu?n 1: GPS placeholder -> chuy?n qua tab Map
        await Shell.Current.GoToAsync("..");
        // N?u route tab map không ph?i MapPage, b?n có th? ch? ??n gi?n:
        // await Shell.Current.GoToAsync(".."); r?i h??ng d?n ? MapPage
    }

    private async void OnManualModeClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ZoneListPage));
    }
}
