using CurrencySystem.Application;
using CurrencySystem.Domain;

namespace ConditionEngine.Infrastructure
{
    /// <summary>
    /// Provider inventory riêng
    /// Kết nối với CurrencySystem / ItemSystem
    /// </summary>
    public class InventoryProvider
    {
        private readonly CurrencyService _currencyService;

        public InventoryProvider(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        /// <summary>
        /// Kiểm tra player có item không
        /// (có thể kết nối ItemSystem sau)
        /// </summary>
        public bool HasItem(string itemId)
        {
            // TODO: connect ItemSystem
            return false;
        }

        /// <summary>
        /// Lấy số lượng currency
        /// </summary>
        public int GetCurrency(string currencyId)
        {
            if (string.IsNullOrEmpty(currencyId))
                return 0;

            var id = new CurrencyId(currencyId);

            return _currencyService.GetBalance(id);
        }
    }
}