using R3;

/// <summary>
/// Xử lý business logic của Setting
/// </summary>
public class SettingService
{
    private readonly SettingState _state;
    private readonly ISettingRepository _repository;
    private readonly IHapticService _haptic;

    public SettingService(
        SettingState state,
        ISettingRepository repository,
        IHapticService haptic)
    {
        _state = state;
        _repository = repository;
        _haptic = haptic;

        BindAutoSave();
    }

    /// <summary>
    /// Tự động lưu mỗi khi state thay đổi
    /// </summary>
    private void BindAutoSave()
    {
        _state.Sound.Subscribe(v => _repository.SaveSound(v));
        _state.Vibration.Subscribe(v => _repository.SaveVibration(v));
        _state.Music.Subscribe(v => _repository.SaveMusic(v));
    }

    public void SetSound(bool value)
    {
        _state.Sound.Value = value;
        TryVibrate();
    }

    public void SetVibration(bool value)
    {
        _state.Vibration.Value = value;
        TryVibrate();
    }

    public void SetMusic(bool value)
    {
        _state.Music.Value = value;
        TryVibrate();
    }

    public void ResetToDefault()
    {
        _state.Sound.Value = true;
        _state.Vibration.Value = true;
        _state.Music.Value = false;
    }

    /// <summary>
    /// Chỉ rung nếu vibration đang bật
    /// </summary>
    private void TryVibrate()
    {
        if (_state.Vibration.Value)
            _haptic.Light();
    }
}