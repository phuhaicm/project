using ZXing.Net.Maui;

namespace PoiNarration.Mobile.Views; // Đảm bảo có .Views ở đây

public partial class QRScanPage : ContentPage
{
    public QRScanPage()
    {
        InitializeComponent();

        // Cấu hình máy quét cho bản cũ
        cameraBarcodeReaderView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormat.QrCode,
            AutoRotate = true,
            Multiple = false
        };
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        cameraBarcodeReaderView.IsDetecting = true;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        cameraBarcodeReaderView.IsDetecting = false;
    }

    // ĐÂY LÀ HÀM QUAN TRỌNG: Tên phải giống hệt bên XAML
    private void CameraBarcodeReaderView_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        var result = e.Results?.FirstOrDefault();
        if (result == null) return;

        cameraBarcodeReaderView.IsDetecting = false;

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            string qrValue = result.Value;
            if (qrValue.StartsWith("booth:"))
            {
                string boothId = qrValue.Replace("booth:", "");

                // NHẢY TRANG: Đưa boothId sang trang chi tiết
                // Dùng dấu ? để truyền tham số boothId lên thanh điều hướng
                await Shell.Current.GoToAsync($"{nameof(BoothDetailPage)}?boothId={boothId}");
            }
        });
    }
}