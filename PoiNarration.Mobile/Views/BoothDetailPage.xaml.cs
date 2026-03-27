using Microsoft.Extensions.DependencyInjection;
using PoiNarration.Core.Models;
using PoiNarration.Mobile.Services;
using System.Net.Http.Json;

namespace PoiNarration.Mobile.Views;

[QueryProperty(nameof(BoothId), "boothId")]
public partial class BoothDetailPage : ContentPage
{
    private readonly AppDatabase _db;
    private readonly NarrationService _narrationService;

    public string BoothId { get; set; } = "";
    private Booth? _currentBooth;

    public BoothDetailPage()
    {
        InitializeComponent();

        var services = Application.Current?.Handler?.MauiContext?.Services
                       ?? throw new Exception("Services is null");

        _db = services.GetRequiredService<AppDatabase>();
        _narrationService = services.GetRequiredService<NarrationService>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _db.InitAsync();

        if (string.IsNullOrWhiteSpace(BoothId)) return;

        // Bật vòng xoay loading lên khi bắt đầu
        LoadingArea.IsVisible = true;

        // 1. HIỂN THỊ THÔNG TIN CƠ BẢN TỪ SQLITE (Ưu tiên nhanh)
        var booth = await _db.GetBoothAsync(BoothId);
        if (booth != null)
        {
            _currentBooth = booth;
            // Thay thế chữ "Đang tải..." bằng tên thật từ máy
            BoothName.Text = LanguageService.IsVi ? booth.NameVi : booth.NameEn;
            BoothDesc.Text = LanguageService.IsVi ? booth.DescVi : booth.DescEn;
        }

        // 2. GỌI API ĐỂ LẤY THỰC ĐƠN TƯƠI MỚI (Lấy từ Server)
        try
        {
            string apiUrl = $"http://10.0.2.2:5174/api/BoothMenu/{BoothId}";

            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(5);

            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var apiMenu = await response.Content.ReadFromJsonAsync<List<BoothMenuItem>>();
                if (apiMenu != null && apiMenu.Count > 0)
                {
                    MenuView.ItemsSource = apiMenu;

                    // XỬ LÝ XONG: Ẩn vòng xoay và thoát hàm
                    LoadingArea.IsVisible = false;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            // Chỉ ghi log ra console, không hiện DisplayAlert để tránh gián đoạn trải nghiệm
            Console.WriteLine($"[API Error]: {ex.Message}");
        }

        // 3. DỰ PHÒNG: Nếu API thất bại (không mạng/server tắt), dùng data SQLite cũ
        var localMenu = await _db.GetMenuByBoothAsync(BoothId);
        MenuView.ItemsSource = localMenu;

        // Hoàn tất tải (dù là dùng data cũ) thì cũng ẩn vòng xoay đi
        LoadingArea.IsVisible = false;
    }

    private async void OnPlayNarrationClicked(object sender, EventArgs e)
    {
        if (_currentBooth == null) return;
        await _narrationService.SpeakBoothAsync(_currentBooth, "Manual");
    }
}