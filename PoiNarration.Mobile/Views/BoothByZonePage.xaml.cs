using Microsoft.Extensions.DependencyInjection;
using PoiNarration.Core.Models;

namespace PoiNarration.Mobile.Views;

[QueryProperty(nameof(ZoneId), "zoneId")]
public partial class BoothByZonePage : ContentPage
{
    private readonly AppDatabase _db;
    private bool _isNavigating = false;

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
        if (_isNavigating) return;

        if (e.CurrentSelection.FirstOrDefault() is not Booth booth)
            return;

        _isNavigating = true;

        // clear selection NGAY để lần sau bấm lại item cũ vẫn ăn
        ((CollectionView)sender).SelectedItem = null;

        try
        {
            await Shell.Current.GoToAsync($"{nameof(BoothDetailPage)}?boothId={booth.Id}");
        }
        finally
        {
            _isNavigating = false;
        }
    }
}
