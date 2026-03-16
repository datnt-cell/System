using Cysharp.Threading.Tasks;
using Gley.EasyIAP;

namespace IAPModule.Application.Interfaces
{
    /// <summary>
    /// Interface provider mua hàng
    /// Infra sẽ implement bằng Gley
    /// </summary>
    public interface IIAPProvider
    {
        UniTask<PurchaseProductResponseData> BuyAsync(ShopProductNames productId);
        UniTask RestoreAsync();
        ProductType GetProductType(ShopProductNames productId);
    }
}