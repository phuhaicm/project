using PoiNarration.Mobile.Services;
using PoiNarration.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace PoiNarration.Mobile.Views;

public partial class BoothListPage : ContentPage
{
    private readonly AppDatabase _db;
    private readonly SeedService _seed;

    public BoothListPage()
    {
        InitializeComponent();

        // Cách đơn giản (không DI phức tạp): lấy service từ App.Current.Services

        var services = Application.Current?.Handler?.MauiContext?.Services;
        if (services == null)
            throw new Exception("ServiceProvider is null. App chưa khởi tạo MauiContext.");

        _db = services.GetRequiredService<AppDatabase>();

        _seed = new SeedService(_db);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _seed.EnsureSeededAsync();

        // Load tất cả booth (tuần 2)
        var booths = await _db.GetAllBoothsAsync();

        // map sang model hiển thị theo ngôn ngữ
        BoothsView.ItemsSource = booths.Select(b => new BoothDisplay
        {
            Id = b.Id,
            Name = LanguageService.IsVi ? b.NameVi : b.NameEn,
            Desc = LanguageService.IsVi ? b.DescVi : b.DescEn
        }).ToList();
    }

    private async void OnScanGateClicked(object sender, EventArgs e)
    {
        // Chuyển hướng sang trang quét mã QR thần thánh!
        await Navigation.PushAsync(new PoiNarration.Mobile.Views.QRScanPage());
    }

    private async void OnManualClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync(nameof(ZoneListPage));

    private async void OnBoothSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not BoothDisplay booth) return;
        await Shell.Current.GoToAsync($"{nameof(BoothDetailPage)}?boothId={booth.Id}");
        ((CollectionView)sender).SelectedItem = null;
    }

    private void OnLangVi(object sender, EventArgs e)
    {
        LanguageService.Set("vi");
        OnAppearing(); // reload
    }

    private void OnLangEn(object sender, EventArgs e)
    {
        LanguageService.Set("en");
        OnAppearing(); // reload
    }
}

public class BoothDisplay
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Desc { get; set; } = "";
}
