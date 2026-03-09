using CurrencySystem.Domain;
using CurrencySystem.Application;
using CurrencySystem.Presentation;
using CurrencySystem.Infrastructure;

namespace CurrencySystem.Installer
{
    /// <summary>
    /// Tạo và connect toàn bộ dependency graph cho Currency system.
    /// </summary>
    public class CurrencyInstaller
    {
        public CurrencyInstallResult Install()
        {
            var config = CurrencyGlobalConfig.Instance;
            var bundle = CurrencyBundleGlobalConfig.Instance;

            ICurrencyMetadataProvider metadataProvider =
                new GlobalConfigCurrencyMetadataProvider(config);

            // =========================
            // DOMAIN
            // =========================

            var state = new CurrencyState(metadataProvider);

            // =========================
            // INFRASTRUCTURE
            // =========================

            var repository = new EasySaveCurrencyRepository();
            var bundleProvider = new GlobalConfigBundleProvider(bundle);

            // =========================
            // APPLICATION
            // =========================

            var service = new CurrencyService(state, repository);
            var bundleUseCase = new CurrencyBundleUseCase(service, bundleProvider);

            // =========================
            // PRESENTATION
            // =========================

            var viewModel = new CurrencyViewModel(state);
            var presenter = new CurrencyPresenter(service, bundleUseCase);

            return new CurrencyInstallResult(
                presenter,
                viewModel,
                service,
                bundleUseCase);
        }
    }
}

/// <summary>
/// DTO chứa kết quả install.
/// </summary>
public readonly struct CurrencyInstallResult
{
    public CurrencyPresenter Presenter { get; }
    public CurrencyViewModel ViewModel { get; }

    public CurrencyService CurrencyService { get; }
    public CurrencyBundleUseCase BundleUseCase { get; }

    public CurrencyInstallResult(
        CurrencyPresenter presenter,
        CurrencyViewModel viewModel,
        CurrencyService currencyService,
        CurrencyBundleUseCase bundleUseCase)
    {
        Presenter = presenter;
        ViewModel = viewModel;
        CurrencyService = currencyService;
        BundleUseCase = bundleUseCase;
    }
}