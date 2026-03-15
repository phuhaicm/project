using Microsoft.Extensions.DependencyInjection;
using PoiNarration.Core.Models;
using PoiNarration.Mobile.Services;

namespace PoiNarration.Mobile.Views;

[QueryProperty(nameof(ZoneId), "zoneId")]
public partial class BoothByZonePage : ContentPage
{
    private readonly AppDatabase _db;

    public string ZoneId { get; set; } = "";

    public BoothByZonePage()
    {
        InitializeComponent();

        var services = Application.Current?.Handler?.MauiContext?.Services
                       ?? throw new Exception("Services is null");

        _db = services.GetRequiredService<AppDatabase>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _db.InitAsync();

        if (!string.IsNullOrWhiteSpace(ZoneId))
        {
            BoothsView.ItemsSource = await _db.GetBoothsByZoneAsync(ZoneId);
        }
    }

    private async void OnBoothSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not Booth booth) return;

        await Shell.Current.GoToAsync($"{nameof(BoothDetailPage)}?boothId={booth.Id}");

        ((CollectionView)sender).SelectedItem = null;
    }
}