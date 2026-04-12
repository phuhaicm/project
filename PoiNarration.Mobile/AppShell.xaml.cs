using PoiNarration.Mobile.Services;
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
        Routing.RegisterRoute("mappage", typeof(MapPage));
        Routing.RegisterRoute("boothdetail", typeof(BoothDetailPage));
        Routing.RegisterRoute("qrscan", typeof(QrScanPage));

        LanguageService.LanguageChanged += RefreshShellTexts;
        RefreshShellTexts();
    }

    private void RefreshShellTexts()
    {
        Title = LanguageService.T("Ui_AppTitle");

        if (BoothsTab != null)
            BoothsTab.Title = LanguageService.T("Ui_Tab_Booths");

        if (MapTab != null)
            MapTab.Title = LanguageService.T("Ui_Tab_Map");

        if (QrTab != null)
            QrTab.Title = LanguageService.T("Ui_Tab_Qr");
    }
}