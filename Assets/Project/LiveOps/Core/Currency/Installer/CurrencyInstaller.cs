using CurrencySystem.Domain;
using CurrencySystem.Application;
using CurrencySystem.Infrastructure;

namespace CurrencySystem.Installer
{
    public class CurrencyInstaller
    {
        public CurrencyInstallResult Install()
        {
            var config = CurrencyGlobalConfig.Instance;
            var bundleConfig = CurrencyBundleGlobalConfig.Instance;

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
            var bundleProvider = new GlobalConfigBundleProvider(bundleConfig);

            // =========================
            // EVENTS
            // =========================
            var currencyEvents = new CurrencyEvents();
            var bundleEvents = new CurrencyBundleEvents();

            // =========================
            // APPLICATION
            // =========================
            var service = new CurrencyService(state, repository, currencyEvents);
            var bundleUseCase = new CurrencyBundleUseCase(service, bundleProvider, bundleEvents);

            return new CurrencyInstallResult(
                service,
                bundleUseCase,
                currencyEvents,
                bundleEvents
            );
        }
    }

    public readonly struct CurrencyInstallResult
    {
        public CurrencyService Service { get; }
        public CurrencyBundleUseCase BundleUseCase { get; }
        public CurrencyEvents CurrencyEvents { get; }
        public CurrencyBundleEvents BundleEvents { get; }

        public CurrencyInstallResult(
            CurrencyService service,
            CurrencyBundleUseCase bundleUseCase,
            CurrencyEvents currencyEvents,
            CurrencyBundleEvents bundleEvents)
        {
            Service = service;
            BundleUseCase = bundleUseCase;
            CurrencyEvents = currencyEvents;
            BundleEvents = bundleEvents;
        }
    }
}