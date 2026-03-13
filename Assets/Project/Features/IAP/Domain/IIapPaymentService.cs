using Cysharp.Threading.Tasks;
using Gley.EasyIAP;
public interface IIapPaymentService
{
    PurchaseProductResponseData ValidatePurchase(ShopProductNames productId);

    UniTask<PurchaseProductResponseData> Purchase(ShopProductNames productId);
}