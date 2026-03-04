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
            ICurrencyMetadataProvider metadataProvider = new GlobalConfigCurrencyMetadataProvider(CurrencyGlobalConfig.Instance);

            // Domain
            var state = new CurrencyState(metadataProvider);

            // Infrastructure
            var repository = new EasySaveCurrencyRepository();

            // Application
            var service = new CurrencyService(state, repository);

            // Presentation
            var viewModel = new CurrencyViewModel(state);
            var presenter = new CurrencyPresenter(service);

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