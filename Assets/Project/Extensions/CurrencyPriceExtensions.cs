using System.Collections.Generic;
using System.Linq;

public static class CurrencyPriceExtensions
{
    public static List<CurrencyPrice> ToPrices(
        this IEnumerable<CurrencyRewardConfig> configs)
    {
        if (configs == null)
            return new List<CurrencyPrice>();

        return configs
            .Select(x => new CurrencyPrice(x))
            .ToList();
    }
}