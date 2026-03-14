using Gley.EasyIAP;
using IAPModule.Application.Interfaces;

namespace IAPModule.Infrastructure.Repositories
{
    /// <summary>
    /// Repository lưu trạng thái IAP bằng Easy Save.
    /// File lưu: IAP_Save.es3
    /// </summary>
    public class EasySaveIAPRepository : IIAPRepository
    {
        const string FILE_NAME = "IAP_Save.es3";

        /// <summary>
        /// Kiểm tra product đã nhận reward chưa
        /// </summary>
        public bool HasRewarded(ShopProductNames productId)
        {
            return ES3.Load(GetKey(productId), FILE_NAME, false);
        }

        /// <summary>
        /// Đánh dấu product đã nhận reward
        /// </summary>
        public void MarkRewarded(ShopProductNames productId)
        {
            ES3.Save(GetKey(productId), true, FILE_NAME);
        }

        /// <summary>
        /// Tạo key lưu trữ cho product
        /// </summary>
        string GetKey(ShopProductNames productId)
        {
            return $"IAP_Rewarded_{productId}";
        }
    }
}