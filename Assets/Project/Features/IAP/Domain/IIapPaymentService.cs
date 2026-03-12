using Cysharp.Threading.Tasks;
using Gley.EasyIAP;
using IAPModule.Domain.Entities;

public interface IIapPaymentService
{
    bool CanPurchase(ShopProductNames productId);

    UniTask<PurchaseResult> Purchase(ShopProductNames productId);
}
