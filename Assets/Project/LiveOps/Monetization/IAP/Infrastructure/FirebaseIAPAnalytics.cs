using Gley.EasyIAP;
using IAPModule.Application.Interfaces;

namespace IAPModule.Infrastructure.Analytics
{
    /// <summary>
    /// Adapter Firebase
    /// </summary>
    public class FirebaseIAPAnalytics : IIAPAnalytics
    {
        public void LogPurchaseShow(ShopProductNames productId)
        {
            //FirebaseEventLogger.LogPurchaseShow(productId);
        }

        public void LogPurchaseResult(ShopProductNames productId, bool success)
        {
            //FirebaseEventLogger.LogPurchaseResult(productId, success);
        }
    }
}