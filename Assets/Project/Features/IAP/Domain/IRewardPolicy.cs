using Gley.EasyIAP;

namespace IAPModule.Domain.Policies
{
    /// <summary>
    /// Policy quyết định có được nhận reward hay không
    /// Ví dụ: Non-consumable chỉ được nhận 1 lần
    /// </summary>
    public interface IRewardPolicy
    {
        bool CanGrant(ShopProductNames productId);
        void MarkGranted(ShopProductNames productId);
    }
}