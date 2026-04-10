namespace PoiNarration.Mobile.Views;

public partial class QrScanPage : ContentPage
{
    public QrScanPage()
    {
        InitializeComponent();
    }

    private async void OnOpenBooth01Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(BoothDetailPage)}?boothId=booth-01");
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}