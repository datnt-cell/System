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

        public IapPriceStrategy(ShopProductNames productId)
        {
            _productId = productId;
        }

        public bool CanPay() => true; // luôn cho phép click

        public void Pay()
        {
            // Gọi IAP Service bên ngoài
        }
    }
}