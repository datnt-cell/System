using Gley.EasyIAP;
using IAPModule.Domain;
using IAPModule.Application.Interfaces;

namespace ConditionEngine.Infrastructure
{
    /// <summary>
    /// Provider cung cấp dữ liệu IAP cho ConditionEngine
    /// </summary>
    public class PurchaseProvider
    {
        private readonly IAPService _iAPService;

        public PurchaseProvider(IAPService iAPService)
        {
            _iAPService = iAPService;

        }

        public float TotalSpend => _iAPService.GetPaymentService().GetTotalSpend();

        public int PurchaseCount => _iAPService.GetPaymentService().GetPurchaseCount();

        public bool HasPurchased(string productId) => _iAPService.GetPaymentService().HasPurchased(productId);

        public bool HasAnyPurchase() => _iAPService.GetPaymentService().HasAnyPurchase();
    }
}