using Cysharp.Threading.Tasks;
using Gley.EasyIAP;
using IAPModule.Application.Interfaces;

namespace IAPModule.Infrastructure.Providers
{
    /// <summary>
    /// Adapter cho Gley Easy IAP
    /// 
    /// Layer: Infrastructure
    /// 
    /// Chịu trách nhiệm:
    /// - Convert API của Gley -> Interface của Application
    /// - Không chứa business logic
    /// </summary>
    public class GleyIAPProvider : IIAPProvider
    {
        private bool _initialized;

        public bool IsInitialized()
        {
            return _initialized;
        }

        public UniTask InitializeAsync()
        {
            var tcs = new UniTaskCompletionSource();

            API.Initialize((status, message) =>
            {
                if (status == IAPOperationStatus.Success)
                {
                    _initialized = true;
                    tcs.TrySetResult();
                }
                else
                {
                    _initialized = false;
                    tcs.TrySetException(new System.Exception(message));
                }
            });

            return tcs.Task;
        }

        /// <summary>
        /// Mua sản phẩm
        /// </summary>
        public UniTask<PurchaseProductResponseData> BuyAsync(ShopProductNames productId)
        {
            var tcs = new UniTaskCompletionSource<PurchaseProductResponseData>();

            API.BuyProduct(productId, (status, message, product) =>
            {
                if (status == IAPOperationStatus.Success)
                {
                    var response = ResponseData.GetSuccessResponse<PurchaseProductResponseData>();

                    response.ProductId = product.GetStoreID();
                    response.PurchasedOnClient = true;

                    tcs.TrySetResult(response);
                }
                else
                {
                    var response = ResponseData.GetErrorResponse<PurchaseProductResponseData>(
                        Errors.UnityPurchasing_PurchaseFailed,
                        message
                    );

                    if (product != null)
                        response.ProductId = product.GetStoreID();

                    tcs.TrySetResult(response);
                }
            });

            return tcs.Task;
        }

        /// <summary>
        /// Restore purchase
        /// (iOS cần, Android thường auto restore)
        /// </summary>
        public UniTask RestoreAsync()
        {
            var tcs = new UniTaskCompletionSource();

            API.RestorePurchases((status, message, product) =>
            {
                if (status == IAPOperationStatus.Success)
                {
                    tcs.TrySetResult();
                }
                else
                {
                    tcs.TrySetException(new System.Exception(message));
                }
            });

            return tcs.Task;
        }

        /// <summary>
        /// Kiểm tra sản phẩm đã được mua chưa
        /// (dùng cho NonConsumable / Subscription)
        /// </summary>
        public bool IsProductOwned(ShopProductNames productId)
        {
            return API.IsActive(productId);
        }

        /// <summary>
        /// Lấy loại product
        /// Consumable / NonConsumable / Subscription
        /// </summary>
        public ProductType GetProductType(ShopProductNames productId)
        {
            return API.GetProductType(productId);
        }

    }
}