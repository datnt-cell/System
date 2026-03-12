using Cysharp.Threading.Tasks;
using Gley.EasyIAP;
using IAPModule.Domain.Entities;

namespace StoreSystem.Domain
{
    /// <summary>
    /// Thanh toán bằng IAP productId.
    /// Infrastructure sẽ xử lý callback.
    /// </summary>
    public class IapPriceStrategy : IPriceStrategy
    {
        private readonly ShopProductNames _productId;
        private readonly IIapPaymentService _paymentService;

        public IapPriceStrategy(
            ShopProductNames productId,
            IIapPaymentService paymentService)
        {
            _productId = productId;
            _paymentService = paymentService;
        }

        public bool CanPay() => _paymentService.CanPurchase(_productId);

        public async UniTask<bool> Pay()
        {
            var result = await _paymentService.Purchase(_productId);
            return result.IsSuccess;
        }
    }
}