using Gley.EasyIAP;

namespace IAPModule.Application.Interfaces
{
    /// <summary>
    /// Abstraction analytics (Firebase, Adjust...)
    /// </summary>
    public interface IIAPAnalytics
    {
        void LogPurchaseShow(ShopProductNames productId);
        void LogPurchaseResult(ShopProductNames productId, bool success);
    }
}