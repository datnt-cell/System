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

            // Domain
            var state = new CurrencyState(metadataProvider);

            // Infrastructure
            var repository = new EasySaveCurrencyRepository();
            var bundleProvider = new GlobalConfigBundleProvider(bundle);

            // Application
            var service = new CurrencyService(state, repository);
            var bundleUseCase = new CurrencyBundleUseCase(service, bundleProvider);

            // Presentation
            var viewModel = new CurrencyViewModel(state);
            var presenter = new CurrencyPresenter(service, bundleUseCase);

            return new CurrencyInstallResult(
                presenter,
                viewModel);
        }
    }

    /// <summary>
    /// DTO chứa kết quả install.
    /// </summary>
    public readonly struct CurrencyInstallResult
    {
        public CurrencyPresenter Presenter { get; }
        public CurrencyViewModel ViewModel { get; }

        public CurrencyInstallResult(
            CurrencyPresenter presenter,
            CurrencyViewModel viewModel)
        {
            Presenter = presenter;
            ViewModel = viewModel;
        }
    }
}