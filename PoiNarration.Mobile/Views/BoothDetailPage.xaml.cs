using Microsoft.Extensions.DependencyInjection;
using PoiNarration.Core.Models;
using PoiNarration.Mobile.Services;
using System.Text.Json;

namespace PoiNarration.Mobile.Views;

// QueryProperty giúp nhận boothId từ Shell Navigation (trang QR bắn qua)
[QueryProperty(nameof(BoothId), "BoothId")]
public partial class BoothDetailPage : ContentPage
{
    // Cấu hình địa chỉ Server (Dùng 10.0.2.2 cho máy ảo Android)
    private const string BaseUrl = "http://10.0.2.2:5231";

    private AppDatabase _db;
    private NarrationService _narrationService;
    private Booth? _currentBooth;

    private string _boothId = "";
    public string BoothId
    {
        get => _boothId;
        set
        {
            _boothId = value;
            // Khi ID vừa "cập bến", lập tức đi lấy dữ liệu ngay
            OnBoothIdReceived(value);
        }
    }

    public BoothDetailPage()
    {
        InitializeComponent();
        SetupServices();
    }

    // Constructor này dùng khi sếp Navigation.PushAsync thủ công
    public BoothDetailPage(string boothId)
    {
        InitializeComponent();
        SetupServices();
        BoothId = boothId;
    }

    private void SetupServices()
    {
        var services = Application.Current?.Handler?.MauiContext?.Services
                       ?? throw new Exception("Services is null");

        _db = services.GetRequiredService<AppDatabase>();
        _narrationService = services.GetRequiredService<NarrationService>();
    }

    private async void OnBoothIdReceived(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return;

        // 1. Lấy thông tin Booth từ Local DB (Tên, Mô tả chung)
        await _db.InitAsync();
        _currentBooth = await _db.GetBoothAsync(id);

        if (_currentBooth != null)
        {
            // Thêm chữ Label vào sau tên biến cho đúng với XAML nhé sếp
            BoothNameLabel.Text = LanguageService.IsVi ? _currentBooth.NameVi : _currentBooth.NameEn;
            BoothDescLabel.Text = LanguageService.IsVi ? _currentBooth.DescVi : _currentBooth.DescEn;
        }

        // 2. Lấy danh sách Menu "Tươi sống" từ API
        await LoadMenuFromApi(id);
    }

    private async Task LoadMenuFromApi(string id)
    {
        try
        {
            using var client = new HttpClient();
            string url = $"{BaseUrl}/api/BoothMenu/{id}";

            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                // Giải mã dữ liệu
                var items = JsonSerializer.Deserialize<List<BoothMenuItem>>(json, options);

                if (items != null)
                {
                    foreach (var item in items)
                    {
                        if (!string.IsNullOrEmpty(item.ImageUrl) && !item.ImageUrl.StartsWith("http"))
                        {
                            item.ImageUrl = $"{BaseUrl}{item.ImageUrl}";
                        }
                    }

                    // 1. CẬP NHẬT GIAO DIỆN PHẢI NẰM TRONG MAIN THREAD
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        MenuView.ItemsSource = null; // Xóa sạch đồ cũ
                        MenuView.ItemsSource = items; // Dọn Phở Gà lên
                    });
                }
            }
        }
        catch (Exception ex)
        {
            // 2. BẬT CẢNH BÁO LÊN ĐỂ XEM NÓ BỊ LỖI GÌ (Thay vì lén lút hiện Burger)
            MainThread.BeginInvokeOnMainThread(() =>
            {
                DisplayAlert("Lỗi API", $"Không tải được Phở: {ex.Message}", "OK");
            });

            // Tạm thời comment dòng này lại để không bị Burger làm lú nữa
            // MenuView.ItemsSource = await _db.GetMenuByBoothAsync(id);
        }
    }

    private async void OnPlayNarrationClicked(object sender, EventArgs e)
    {
        if (_currentBooth == null) return;
        await _narrationService.SpeakBoothAsync(_currentBooth, "Manual");
    }

    private async void OnMenuItemSelected(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = e.CurrentSelection.FirstOrDefault() as BoothMenuItem;
        if (selectedItem == null) return;

        // Logic đọc tiếng Việt "xịn" của sếp
        string textToRead = $"{selectedItem.Name}. {selectedItem.Description}. Giá {selectedItem.Price} VNĐ.";
        var locales = await TextToSpeech.Default.GetLocalesAsync();
        var vietnamese = locales.FirstOrDefault(l => l.Language == "vi");

        var options = new SpeechOptions()
        {
            Locale = vietnamese,
            Pitch = 1.0f,
            Volume = 1.0f
        };

        await TextToSpeech.Default.SpeakAsync(textToRead, options);

        // Bỏ chọn để có thể bấm lại món đó
        ((CollectionView)sender).SelectedItem = null;
    }
}