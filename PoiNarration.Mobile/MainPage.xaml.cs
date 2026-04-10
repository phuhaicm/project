using PoiNarration.Mobile.Services;

namespace PoiNarration.Mobile
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object? sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
        private void OnLanguageChanged(object sender, EventArgs e)
        {
            if (LanguagePicker.SelectedItem is string lang)
            {
                LanguageService.CurrentLanguage = lang;
            }
        }
    }
}
