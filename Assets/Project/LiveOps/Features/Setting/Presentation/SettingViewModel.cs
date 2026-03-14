using R3;

/// <summary>
/// ViewModel chỉ expose state cho View
/// </summary>
public class SettingViewModel
{
    public ReadOnlyReactiveProperty<bool> Sound { get; }
    public ReadOnlyReactiveProperty<bool> Vibration { get; }
    public ReadOnlyReactiveProperty<bool> Music { get; }

    public SettingViewModel(SettingState state)
    {
        Sound = state.Sound;
        Vibration = state.Vibration;
        Music = state.Music;
    }
}