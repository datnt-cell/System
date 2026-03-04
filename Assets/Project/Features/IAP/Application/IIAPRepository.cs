using Gley.EasyIAP;

namespace IAPModule.Application.Interfaces
{
    /// <summary>
    /// Lưu trạng thái đã nhận reward hay chưa
    /// </summary>
    public interface IIAPRepository
    {
        bool HasRewarded(ShopProductNames productId);
        void MarkRewarded(ShopProductNames productId);
    }
}