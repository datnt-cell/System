using Cysharp.Threading.Tasks;
using IAPModule.Domain.Policies;
using IAPModule.Application.Interfaces;
using Gley.EasyIAP;

namespace IAPModule.Application.UseCases
{
    /// <summary>
    /// UseCase xử lý flow mua hàng
    /// Không biết Unity
    /// Không biết Gley
    /// </summary>
    public class PurchaseUseCase
    {
        private readonly IIAPProvider _provider;
        private readonly IIAPRepository _repository;
        private readonly IIAPAnalytics _analytics;
        private readonly IRewardPolicy _rewardPolicy;

        public PurchaseUseCase(
            IIAPProvider provider,
            IIAPRepository repository,
            IIAPAnalytics analytics,
            IRewardPolicy rewardPolicy)
        {
            _provider = provider;
            _repository = repository;
            _analytics = analytics;
            _rewardPolicy = rewardPolicy;
        }

        /// <summary>
        /// Kiểm tra product có thể mua hay không
        /// </summary>
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

        /// <summary>
        /// Thực hiện mua hàng
        /// </summary>
        public async UniTask<PurchaseProductResponseData> ExecuteAsync(ShopProductNames productId)
        {
            // Log show popup
            _analytics.LogPurchaseShow(productId);

            // Validate trước khi mua
            var validation = ValidatePurchase(productId);
            if (!validation.Success)
                return validation;

            // Gọi provider mua hàng
            var result = await _provider.BuyAsync(productId);

            // Log kết quả
            _analytics.LogPurchaseResult(productId, result.Success);

            if (!result.Success)
                return result;

            // Kiểm tra policy reward
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