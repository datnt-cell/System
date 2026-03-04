using System.Collections.Generic;
using System.Linq;
using CurrencySystem.Domain;

namespace CurrencySystem.Infrastructure
{
    public class GlobalConfigBundleProvider : ICurrencyBundleProvider
    {
        private readonly Dictionary<string, CurrencyBundle> _lookup;

        public GlobalConfigBundleProvider(CurrencyGlobalConfig config)
        {
            _lookup = config.Bundles.ToDictionary(
                b => b.Id,
                b => new CurrencyBundle(
                    b.Id,
                    b.Rewards.Select(r =>
                        new CurrencyReward(
                            new CurrencyId(r.CurrencyId),
                            r.Amount)).ToList()));
        }

        public CurrencyBundle GetBundle(string id)
            => _lookup[id];
    }
}