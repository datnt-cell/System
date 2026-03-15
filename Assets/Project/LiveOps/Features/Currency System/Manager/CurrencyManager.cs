using UnityEngine;
using CurrencySystem.Presentation;
using CurrencySystem.Installer;
using CurrencySystem.Application;

/// <summary>
/// Đại diện cho một thực thể (entity) trong game,
/// ví dụ: kiếm, khiên, gỗ, đá hoặc bất kỳ vật phẩm nào.
/// </summary>
public class CurrencyManager : MonoBehaviour
{
    public CurrencyViewModel ViewModel { get; private set; }
    public CurrencyPresenter Presenter { get; private set; }
    public CurrencyService Service { get; private set; }
    public CurrencyBundleUseCase BundleUseCase { get; private set; }

    private CurrencyInstaller _installer;

    public void Initialize()
    {
        _installer = new CurrencyInstaller();

        var result = _installer.Install();

        Presenter = result.Presenter;
        ViewModel = result.ViewModel;

        Service = result.CurrencyService;
        BundleUseCase = result.BundleUseCase;
    }
}