using Cysharp.Threading.Tasks;
using Gley.EasyIAP;
using IAPModule.Application.Interfaces;
using UnityEngine;

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

        public async UniTask InitializeAsync()
        {
            if (_initialized)
                return;

            var tcs = new UniTaskCompletionSource();

            API.Initialize((status, message) =>
            {
                if (status == IAPOperationStatus.Success)
                {
                    _initialized = true;
                    Debug.Log("IAP Initialized");
                }
                else
                {
                    Debug.LogWarning($"IAP Init Failed: {message}");
                }

                tcs.TrySetResult();
            });

            var result = await UniTask.WhenAny(
                tcs.Task,
                UniTask.Delay(1000)
            );

            if (result == 1)
                Debug.LogWarning("IAP Init Timeout (continue game)");
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

                    response.ProductId = productId.ToString();
                    response.Price = (float)API.GetPrice(productId);
                    response.Currency = API.GetIsoCurrencyCode(productId);
                    response.LocalizedPrice = API.GetLocalizedPriceString(productId);
                    response.Receipt = API.GetReceipt(productId);
                    response.RewardValue = API.GetValue(productId);

                    response.PurchasedOnClient = true;

                    Debug.Log($"[IAP] Purchase Success: {productId}");

                    tcs.TrySetResult(response);
                }
                else
                {
                    var response = ResponseData.GetErrorResponse<PurchaseProductResponseData>(
                           Errors.UnityPurchasing_PurchaseFailed,
                           message
                       );

                    response.ProductId = productId.ToString();

                    Debug.LogWarning($"[IAP] Purchase Failed: {message}");

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
                    Debug.Log("[IAP] Restore success");
                    tcs.TrySetResult();
                }
                else
                {
                    Debug.LogWarning($"[IAP] Restore failed: {message}");
                    tcs.TrySetException(new System.Exception(message));
                }
            });

            return tcs.Task;
        }

        // =================================
        // PRODUCT INFO
        // =================================

        public bool IsProductOwned(ShopProductNames productId)
        {
            return API.IsActive(productId);
        }

        public ProductType GetProductType(ShopProductNames productId)
        {
            return API.GetProductType(productId);
        }

        public float GetPrice(ShopProductNames productId)
        {
            return (float)API.GetPrice(productId);
        }

        public string GetCurrency(ShopProductNames productId)
        {
            return API.GetIsoCurrencyCode(productId);
        }

        public string GetLocalizedPrice(ShopProductNames productId)
        {
            return API.GetLocalizedPriceString(productId);
        }

        public string GetReceipt(ShopProductNames productId)
        {
            return API.GetReceipt(productId);
        }

        public int GetRewardValue(ShopProductNames productId)
        {
            return API.GetValue(productId);
        }
    }
}