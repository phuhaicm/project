using PoiNarration.Mobile.Views;

namespace PoiNarration.Mobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(QrScanPage), typeof(QrScanPage));
        Routing.RegisterRoute(nameof(GateModePage), typeof(GateModePage));
        Routing.RegisterRoute(nameof(ZoneListPage), typeof(ZoneListPage));
        Routing.RegisterRoute(nameof(BoothListPage), typeof(BoothListPage));
        Routing.RegisterRoute(nameof(BoothDetailPage), typeof(BoothDetailPage));
        Routing.RegisterRoute(nameof(BoothByZonePage), typeof(BoothByZonePage));
        Routing.RegisterRoute("mappage", typeof(PoiNarration.Mobile.Views.MapPage));
        Routing.RegisterRoute("boothdetail", typeof(PoiNarration.Mobile.Views.BoothDetailPage));
        Routing.RegisterRoute("qrscan", typeof(PoiNarration.Mobile.Views.QrScanPage));

    }
}