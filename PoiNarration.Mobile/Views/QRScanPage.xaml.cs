using ZXing;
using ZXing.Net.Maui;

namespace PoiNarration.Mobile.Views;

public partial class QRScanPage : ContentPage
{
    public QRScanPage()
    {
        InitializeComponent();

        // Ép nó chỉ quét mã QR 2D cho tốc độ bàn thờ
        barcodeReader.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.TwoDimensional,
            AutoRotate = true,
            Multiple = false // Chỉ bắt 1 mã đầu tiên nhìn thấy
        };
        Task.Run(async () => {
            while (true)
            {
                await ScanLine.TranslateTo(0, 245, 2000, Easing.Linear);
                await ScanLine.TranslateTo(0, 0, 2000, Easing.Linear);
            }
        });
    }

    // Đảm bảo mỗi lần mở trang này là mắt thần được bật
    protected override void OnAppearing()
    {
        base.OnAppearing();
        barcodeReader.IsDetecting = true;
    }

    // Sự kiện khi camera chộp được mã
    private void barcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        var first = e.Results?.FirstOrDefault();
        if (first == null) return;

        // Lấy chuỗi từ QR Code (Sẽ là: "booth:booth-01")
        string rawResult = first.Value;

        // DÙNG LỆNH NÀY ĐỂ GỌT BỎ CHỮ "booth:" THỪA THÃI
        string finalBoothId = rawResult.Replace("booth:", "");

        // Gửi ID sạch sẽ ("booth-01") sang trang Chi tiết
        Dispatcher.Dispatch(async () =>
        {
            barcodeReader.IsDetecting = false;
            await Shell.Current.GoToAsync($"{nameof(BoothDetailPage)}?BoothId={finalBoothId}");
        });
    }
}