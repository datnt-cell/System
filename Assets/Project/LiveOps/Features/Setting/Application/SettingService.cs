using R3;
using System;

/// <summary>
/// Xử lý business logic của Setting
/// </summary>
public class SettingService : IDisposable
{
    private readonly SettingState _state;
    private readonly ISettingRepository _repository;
    private readonly IHapticService _haptic;

    private readonly CompositeDisposable _disposables = new();

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
        _state.Sound
            .DistinctUntilChanged()
            .Subscribe(v => _repository.SaveSound(v))
            .AddTo(_disposables);

        _state.Vibration
            .DistinctUntilChanged()
            .Subscribe(v => _repository.SaveVibration(v))
            .AddTo(_disposables);

        _state.Music
            .DistinctUntilChanged()
            .Subscribe(v => _repository.SaveMusic(v))
            .AddTo(_disposables);
    }

    public void SetSound(bool value)
    {
        if (_state.Sound.Value == value)
            return;

        _state.Sound.Value = value;
        TryVibrate();
    }

    public void SetVibration(bool value)
    {
        if (_state.Vibration.Value == value)
            return;

        _state.Vibration.Value = value;
        TryVibrate();
    }

    public void SetMusic(bool value)
    {
        if (_state.Music.Value == value)
            return;

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

    public void Dispose()
    {
        _disposables.Dispose();
    }
}