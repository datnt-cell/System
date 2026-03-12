using Cysharp.Threading.Tasks;
using CurrencySystem.Application;
using CurrencySystem.Domain;

namespace StoreSystem.Domain
{
    /// <summary>
    /// Thanh toán bằng currency trong game.
    /// </summary>
    public class CurrencyPriceStrategy : IPriceStrategy
    {
        private readonly CurrencyService _currencyService;
        private readonly CurrencyId _currencyId;
        private readonly int _amount;

        public CurrencyPriceStrategy(
            CurrencyService currencyService,
            CurrencyId currencyId,
            int amount)
        {
            _currencyService = currencyService;
            _currencyId = currencyId;
            _amount = amount;
        }

        public bool CanPay()
        {
            return _currencyService.HasEnough(_currencyId, _amount);
        }

        public UniTask<bool> Pay()
        {
            if (!CanPay())
                return UniTask.FromResult(false);

            _currencyService.Spend(_currencyId, _amount, "store_item");

            return UniTask.FromResult(true);
        }
    }
}