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

        public PurchaseProductResponseData ValidatePayment()
        {
            if (!_currencyService.HasEnough(_currencyId, _amount))
            {
                return ResponseData.GetErrorResponse<PurchaseProductResponseData>(
                    Errors.NotEnoughResources,
                    $"Not enough {_currencyId}"
                );
            }

            return ResponseData.GetSuccessResponse<PurchaseProductResponseData>();
        }

        public UniTask<PurchaseProductResponseData> ExecutePayment()
        {
            var validation = ValidatePayment();

            if (!validation.Success)
                return UniTask.FromResult(validation);

            _currencyService.Spend(_currencyId, _amount, "store_item");

            var result = ResponseData.GetSuccessResponse<PurchaseProductResponseData>();

            return UniTask.FromResult(result);
        }
    }
}