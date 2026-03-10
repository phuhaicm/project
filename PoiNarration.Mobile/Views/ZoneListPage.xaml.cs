using PoiNarration.Mobile.Services;
using PoiNarration.Core.Models;

namespace PoiNarration.Mobile.Views;

public partial class ZoneListPage : ContentPage
{
    private readonly AppDatabase _db;

    public ZoneListPage(AppDatabase db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _db.InitAsync();
        ZonesView.ItemsSource = await _db.GetZonesAsync();
    }

    private async void OnZoneSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not PoiNarration.Core.Models.Zone zone) return;

        await Shell.Current.GoToAsync($"{nameof(BoothByZonePage)}?zoneId={zone.Id}");
        ((CollectionView)sender).SelectedItem = null;
    }
}