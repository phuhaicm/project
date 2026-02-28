using PoiNarration.Mobile.Views;

namespace PoiNarration.Mobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(PoiDetailPage), typeof(PoiDetailPage));
    }
}