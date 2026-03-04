using Cysharp.Threading.Tasks;
using Gley.EasyIAP;
using IAPModule.Domain.Entities;

namespace IAPModule.Application.Interfaces
{
    /// <summary>
    /// Interface provider mua hàng
    /// Infra sẽ implement bằng Gley
    /// </summary>
    public interface IIAPProvider
    {
        UniTask<PurchaseResult> BuyAsync(ShopProductNames productId);
        UniTask RestoreAsync();
    }
}