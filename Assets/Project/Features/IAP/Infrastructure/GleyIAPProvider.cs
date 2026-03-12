using Cysharp.Threading.Tasks;
using Gley.EasyIAP;
using IAPModule.Application.Interfaces;
using IAPModule.Domain.Entities;

namespace IAPModule.Infrastructure.Providers
{
    /// <summary>
    /// Adapter Gley
    /// Chỉ nằm ở Infra
    /// </summary>
    public class GleyIAPProvider : IIAPProvider
    {
        public UniTask<PurchaseResult> BuyAsync(ShopProductNames productId)
        {
            var tcs = new UniTaskCompletionSource<PurchaseResult>();

            API.BuyProduct(productId, (status, message, product) =>
            {
                bool success = status == IAPOperationStatus.Success;
                tcs.TrySetResult(new PurchaseResult(product.GetStoreID(), success));
            });

            return tcs.Task;
        }

        public UniTask RestoreAsync()
        {
            API.RestorePurchases(null, null);
            return UniTask.CompletedTask;
        }

        public ProductType GetProductType(ShopProductNames productId)
        {
            var product = API.GetProductType(productId);
            return product;
        }
    }
}