using Cysharp.Threading.Tasks;
using CurrencySystem.Application;
using CurrencySystem.Domain;
using System.Collections.Generic;

namespace StoreSystem.Domain
{
    /// <summary>
    /// Thanh toán bằng currency trong game.
    /// </summary>
    public class CurrencyPriceStrategy : IPriceStrategy
    {
        private readonly CurrencyService _currencyService;
        private readonly List<CurrencyPrice> _prices;

        public CurrencyPriceStrategy(
            CurrencyService currencyService,
            List<CurrencyPrice> prices)
        {
            _currencyService = currencyService;
            _prices = prices;
        }

        public PurchaseProductResponseData ValidatePayment()
        {
            foreach (var price in _prices)
            {
                if (!_currencyService.HasEnough(price.CurrencyId, price.Amount))
                {
                    return ResponseData.GetErrorResponse<PurchaseProductResponseData>(
                        Errors.NotEnoughResources,
                        $"Not enough {price.CurrencyId}"
                    );
                }
            }

            return ResponseData.GetSuccessResponse<PurchaseProductResponseData>();
        }


        public UniTask<PurchaseProductResponseData> ExecutePayment()
        {
            var validation = ValidatePayment();

            if (!validation.Success)
                return UniTask.FromResult(validation);

            foreach (var price in _prices)
            {
                _currencyService.Spend(
                    price.CurrencyId,
                    price.Amount,
                    "store_item"
                );
            }

            var result = ResponseData.GetSuccessResponse<PurchaseProductResponseData>();

            return UniTask.FromResult(result);
        }
    }
}