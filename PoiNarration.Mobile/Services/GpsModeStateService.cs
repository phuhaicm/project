namespace PoiNarration.Mobile.Services;

public class GpsModeStateService
{
    public bool IsEnabled { get; private set; }

    public event EventHandler<bool>? Changed;

    public void SetEnabled(bool enabled)
    {
        if (IsEnabled == enabled)
            return;

        IsEnabled = enabled;
        Changed?.Invoke(this, enabled);
    }
}