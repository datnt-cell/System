using Cysharp.Threading.Tasks;
using Gley.EasyIAP;

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

        public PurchaseProductResponseData ValidatePayment()
        {
            return _paymentService.ValidatePurchase(_productId);
        }

        public async UniTask<PurchaseProductResponseData> ExecutePayment()
        {
            return await _paymentService.Purchase(_productId);
        }
    }
}