using R3;

/// <summary>
/// State thuần domain, chỉ chứa dữ liệu.
/// Không phụ thuộc Unity.
/// </summary>
public class SettingState
{
    public ReactiveProperty<bool> Sound { get; }
    public ReactiveProperty<bool> Vibration { get; }
    public ReactiveProperty<bool> Music { get; }

    public SettingState(bool sound, bool vibration, bool music)
    {
        Sound = new ReactiveProperty<bool>(sound);
        Vibration = new ReactiveProperty<bool>(vibration);
        Music = new ReactiveProperty<bool>(music);
    }
}