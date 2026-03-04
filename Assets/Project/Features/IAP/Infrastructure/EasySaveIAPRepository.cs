using Gley.EasyIAP;
using IAPModule.Application.Interfaces;

namespace IAPModule.Infrastructure.Repositories
{
    /// <summary>
    /// Lưu trạng thái IAP bằng Easy Save
    /// </summary>
    public class EasySaveIAPRepository : IIAPRepository
    {
        public bool HasRewarded(ShopProductNames productId)
        {
            return ES3.Load("IAP_Rewarded_" + productId, false);
        }

        public void MarkRewarded(ShopProductNames productId)
        {
            ES3.Save("IAP_Rewarded_" + productId, true);
        }
    }
}