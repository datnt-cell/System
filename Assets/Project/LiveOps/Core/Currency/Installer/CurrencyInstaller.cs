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
            // EVENTS
            // =========================

            var events = new CurrencyEvents();

            // =========================
            // APPLICATION
            // =========================

            var service = new CurrencyService(state, repository, events);
            var bundleUseCase = new CurrencyBundleUseCase(service, bundleProvider);

            return new CurrencyInstallResult(
                service,
                bundleUseCase,
                events);
        }
    }
}

public readonly struct CurrencyInstallResult
{
    public CurrencyService Service { get; }
    public CurrencyBundleUseCase BundleUseCase { get; }
    public CurrencyEvents Events { get; }

    public CurrencyInstallResult(
        CurrencyService service,
        CurrencyBundleUseCase bundleUseCase,
        CurrencyEvents events)
    {
        Service = service;
        BundleUseCase = bundleUseCase;
        Events = events;
    }
}