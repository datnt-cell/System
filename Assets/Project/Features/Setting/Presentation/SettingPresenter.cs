using R3;
using System;

/// <summary>
/// Presenter kết nối ViewModel và Service.
/// Có thể expose API cho bên ngoài gọi.
/// </summary>
public class SettingPresenter : IDisposable
{
    private readonly SettingViewModel _viewModel;
    private readonly SettingService _service;

    private readonly CompositeDisposable _disposables = new();

    public SettingPresenter(
        SettingViewModel viewModel,
        SettingService service)
    {
        _viewModel = viewModel;
        _service = service;
    }

    // =====================================================
    // Expose State (Readonly)
    // =====================================================

    public ReadOnlyReactiveProperty<bool> Sound => _viewModel.Sound;
    public ReadOnlyReactiveProperty<bool> Vibration => _viewModel.Vibration;
    public ReadOnlyReactiveProperty<bool> Music => _viewModel.Music;

    // =====================================================
    // Các hàm tương ứng Repository (nhưng gọi qua Service)
    // =====================================================

    /// <summary>
    /// Bật/tắt sound
    /// </summary>
    public void SetSound(bool value)
    {
        _service.SetSound(value);
    }

    /// <summary>
    /// Bật/tắt vibration
    /// </summary>
    public void SetVibration(bool value)
    {
        _service.SetVibration(value);
    }

    /// <summary>
    /// Bật/tắt music
    /// </summary>
    public void SetMusic(bool value)
    {
        _service.SetMusic(value);
    }

    /// <summary>
    /// Reset về mặc định
    /// </summary>
    public void Reset()
    {
        _service.ResetToDefault();
    }


    public void Dispose()
    {
        _disposables.Dispose();
    }
}