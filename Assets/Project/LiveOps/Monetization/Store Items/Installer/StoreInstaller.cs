using System.Collections.Generic;
using StoreSystem.Application;
using StoreSystem.Domain;
using StoreSystem.Infrastructure;
using CurrencySystem.Application;

namespace StoreSystem.Installer
{
    public class StoreInstaller
    {
        private readonly CurrencyService _currencyService;
        private readonly CurrencyBundleUseCase _bundleUseCase;

        public StoreInstaller(
            CurrencyService currencyService,
            CurrencyBundleUseCase bundleUseCase)
        {
            _currencyService = currencyService;
            _bundleUseCase = bundleUseCase;
        }

        public StoreInstallerResult Install()
        {
            var globalConfig = StoreItemsGlobalConfig.Instance;

            var items = new List<StoreItem>();

            foreach (var config in globalConfig.Items)
            {
                var priceStrategy = CreatePriceStrategy(config);
                var rewardStrategy = CreateRewardStrategy(config);

                var item = new StoreItem(
                    config.Id,
                    priceStrategy,
                    rewardStrategy);

                items.Add(item);
            }

            var useCase = new StoreItemUseCase(items);

            var events = new StoreEvents();

            return new StoreInstallerResult(
                useCase,
                events
            );
        }

        // =========================
        // PRICE
        // =========================

        private IPriceStrategy CreatePriceStrategy(StoreItemConfigData config)
        {
            return config.PriceType switch
            {
                StorePriceType.Free =>
                    new FreePriceStrategy(),

                StorePriceType.IAP =>
                    new IapPriceStrategy(
                        config.ProductId,
                        GameManager.Instance.IAPManager.Service
                    ),

                StorePriceType.Currency =>
                    new CurrencyPriceStrategy(
                        GameManager.Instance.Currency.Service,
                        CurrencyPriceExtensions.ToPrices(config.CurrencyPrices)
                    ),

                _ => throw new System.Exception("Unknown price type")
            };
        }

        // =========================
        // REWARD
        // =========================

        private IRewardStrategy CreateRewardStrategy(StoreItemConfigData config)
        {
            if (config.RewardMode == StoreRewardMode.UseExistingBundle)
            {
                return new BundleRewardStrategy(
                    _bundleUseCase,
                    config.RewardBundleId);
            }

            return new CustomBundleRewardStrategy(
                _currencyService,
                config.CustomRewards);
        }
    }
}