namespace PoiNarration.Mobile.Views;

public partial class PoiListPage : ContentPage
{
    public PoiListPage()
    {
        InitializeComponent();
    }

    private async void OnOpenDetailClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PoiDetailPage));
    }
}