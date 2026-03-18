using System.Collections.Generic;
using System.Linq;
using CurrencySystem.Domain;

namespace CurrencySystem.Infrastructure
{
    /// <summary>
    /// Metadata provider dùng GlobalConfig.
    /// Cung cấp thông tin MaxStack, DisplayName, tồn tại và loại ItemType.
    /// </summary>
    public class GlobalConfigCurrencyMetadataProvider : ICurrencyMetadataProvider
    {
        private readonly Dictionary<string, CurrencyConfigData> _lookup;

        public GlobalConfigCurrencyMetadataProvider(CurrencyGlobalConfig config)
        {
            _lookup = config.Currencies
                .ToDictionary(x => x.Id, x => x);
        }

        public bool Exists(CurrencyId id)
            => _lookup.ContainsKey(id.Value);

        public int GetMaxStack(CurrencyId id)
        {
            if (!_lookup.TryGetValue(id.Value, out var data))
                return int.MaxValue;

            return data.MaxStack <= 0 ? int.MaxValue : data.MaxStack;
        }

        public string GetDisplayName(CurrencyId id)
            => _lookup.TryGetValue(id.Value, out var data) ? data.DisplayName : id.Value;

        public ItemType GetItemType(CurrencyId id)
            => _lookup.TryGetValue(id.Value, out var data) ? data.Type : ItemType.Currency;

        // =========================
        // Helper: lọc inventory theo type
        // =========================

        public IEnumerable<CurrencyConfigData> GetCurrenciesInventory()
            => _lookup.Values.Where(x => x.Type == ItemType.Currency);

        public IEnumerable<CurrencyConfigData> GetEventItemsInventory()
            => _lookup.Values.Where(x => x.Type == ItemType.Event);

        public IEnumerable<CurrencyConfigData> GetItemsInventory()
            => _lookup.Values.Where(x => x.Type == ItemType.Item);
    }
}