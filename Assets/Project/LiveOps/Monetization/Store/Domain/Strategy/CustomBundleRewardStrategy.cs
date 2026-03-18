using CurrencySystem.Application;
using CurrencySystem.Domain;
using System.Collections.Generic;
using StoreSystem.Domain;

public class CustomBundleRewardStrategy : IRewardStrategy
{
    private readonly CurrencyService _currencyService;
    private readonly List<CurrencyRewardConfig> _rewards;

    public CustomBundleRewardStrategy(
        CurrencyService currencyService,
        List<CurrencyRewardConfig> rewards)
    {
        _currencyService = currencyService;
        _rewards = rewards;
    }

    public void Grant()
    {
        foreach (var reward in _rewards)
        {
            _currencyService.AddCurrency(
                new CurrencyId(reward.CurrencyId),
                reward.Amount,
                reward.CurrencyId.ToString());
        }
    }
}