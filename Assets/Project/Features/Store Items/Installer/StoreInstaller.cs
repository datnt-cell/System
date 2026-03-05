using StoreSystem.Domain;
using StoreSystem.Application;
using CurrencySystem.Application;

public class StoreInstaller
{
    public StoreItemUseCase Create(
        StoreItemConfigData config,
        CurrencyBundleUseCase bundleUseCase,
        CurrencyService currencyService)
    {
        // =========================
        // PRICE
        // =========================

        IPriceStrategy priceStrategy = config.PriceType switch
        {
            StorePriceType.Free => new FreePriceStrategy(),
            StorePriceType.IAP => new IapPriceStrategy(config.ProductId),
            _ => throw new System.Exception("Unknown price type")
        };

        // =========================
        // REWARD
        // =========================

        IRewardStrategy rewardStrategy;

        if (config.RewardMode == StoreRewardMode.UseExistingBundle)
        {
            rewardStrategy = new BundleRewardStrategy(
                bundleUseCase,
                config.RewardBundleId);
        }
        else
        {
            rewardStrategy = new CustomBundleRewardStrategy(
                currencyService,
                config.CustomRewards);
        }

        var storeItem = new StoreItem(
            config.Id,
            config.DisplayName,
            priceStrategy,
            rewardStrategy);

        return new StoreItemUseCase(storeItem);
    }
}