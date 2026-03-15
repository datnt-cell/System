using Cysharp.Threading.Tasks;
using IAPModule.Domain.Policies;
using IAPModule.Application.Interfaces;
using Gley.EasyIAP;

namespace IAPModule.Application.UseCases
{
    public class PurchaseUseCase
    {
        private readonly IIAPProvider _provider;
        private readonly IIAPRepository _repository;
        private readonly IIAPAnalytics _analytics;
        private readonly IRewardPolicy _rewardPolicy;
        private readonly IPaymentService _paymentService;

        public IPaymentService GetPaymentService()=> _paymentService;
        
        public PurchaseUseCase(
            IIAPProvider provider,
            IIAPRepository repository,
            IIAPAnalytics analytics,
            IRewardPolicy rewardPolicy,
            IPaymentService paymentService)
        {
            _provider = provider;
            _repository = repository;
            _analytics = analytics;
            _rewardPolicy = rewardPolicy;
            _paymentService = paymentService;
        }

        public PurchaseProductResponseData ValidatePurchase(ShopProductNames productId)
        {
            if (_provider.GetProductType(productId) == ProductType.NonConsumable)
            {
                if (!_rewardPolicy.CanGrant(productId))
                {
                    return ResponseData.GetErrorResponse<PurchaseProductResponseData>(
                        Errors.NotAvailable,
                        "Product already purchased"
                    );
                }
            }

            return ResponseData.GetSuccessResponse<PurchaseProductResponseData>();
        }

        public async UniTask<PurchaseProductResponseData> ExecuteAsync(ShopProductNames productId)
        {
            // Analytics show
            _analytics.LogPurchaseShow(productId);

            var validation = ValidatePurchase(productId);
            if (!validation.Success)
                return validation;

            // Call store
            var result = await _provider.BuyAsync(productId);

            // Analytics result
            _analytics.LogPurchaseResult(productId, result.Success);

            if (!result.Success)
                return result;

            // =========================
            // UPDATE PAYMENT STATE
            // =========================

            _paymentService.RegisterPurchase(
                result.Price,
                result.Currency
            );

            // =========================
            // REWARD POLICY
            // =========================

            if (_provider.GetProductType(productId) == ProductType.NonConsumable)
            {
                if (_rewardPolicy.CanGrant(productId))
                {
                    _repository.MarkRewarded(productId);
                    _rewardPolicy.MarkGranted(productId);
                }
            }

            return result;
        }
    }
}