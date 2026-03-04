using System.Collections.Generic;
using System.Linq;
using CurrencySystem.Domain;

namespace CurrencySystem.Infrastructure
{
    /// <summary>
    /// Metadata provider dùng GlobalConfig.
    /// Đây là nơi duy nhất dùng Unity data.
    /// </summary>
    public class GlobalConfigCurrencyMetadataProvider 
        : ICurrencyMetadataProvider
    {
        private readonly Dictionary<string, CurrencyConfigData> _lookup;

        public GlobalConfigCurrencyMetadataProvider(
            CurrencyGlobalConfig config)
        {
            _lookup = config.Currencies
                .ToDictionary(x => x.Id, x => x);
        }

        public bool Exists(CurrencyId id)
            => _lookup.ContainsKey(id.Value);

        public int GetMaxStack(CurrencyId id)
            => _lookup.TryGetValue(id.Value, out var data)
                ? data.MaxStack
                : int.MaxValue;

        public string GetDisplayName(CurrencyId id)
            => _lookup.TryGetValue(id.Value, out var data)
                ? data.DisplayName
                : id.Value;
    }
}