//using AndroidX.AppCompat.View.Menu;
using PoiNarration.Core.Models;
using PoiNarration.Mobile.Services;

namespace PoiNarration.Mobile.Views;

public partial class BoothDetailPage : ContentPage, IQueryAttributable
{
    private readonly AppDatabase _db;
    private string _boothId = "";

    public BoothDetailPage(AppDatabase db)
    {
        InitializeComponent();
        _db = db;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("boothId", out var boothIdObj))
            _boothId = boothIdObj?.ToString() ?? "";
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _db.InitAsync();

        var booth = await _db.GetBoothAsync(_boothId);
        if (booth == null) return;

        BoothName.Text = LanguageService.IsVi ? booth.NameVi : booth.NameEn;
        BoothDesc.Text = LanguageService.IsVi ? booth.DescVi : booth.DescEn;

        MenuView.ItemsSource = await _db.GetMenuByBoothAsync(_boothId);
    }
}