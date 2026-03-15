using UnityEngine;
using StoreSystem.Application;
using StoreSystem.Installer;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace StoreSystem.Presentation
{
    /// <summary>
    /// Danh sách các vật phẩm có thể bán trực tiếp trong Shop hoặc thông qua Game Offers.
    /// </summary>
    public class StoreItemsManager : MonoBehaviour
    {
        public StoreItemUseCase UseCase { get; private set; }

        /// <summary>
        /// Khởi tạo Store System
        /// </summary>
        public void Initialize(CurrencyManager currencyManager)
        {
            var installer = new StoreInstaller(
                currencyManager.Service,
                currencyManager.BundleUseCase
            );

            var result = installer.Install();

            UseCase = result.UseCase;
        }

        /// <summary>
        /// Mua item trong Store
        /// </summary>
        public async UniTask<PurchaseProductResponseData> PurchaseStoreItem(string itemId)
        {
            if (UseCase == null)
            {
                Debug.LogError("StoreItemsManager not initialized");

                return ResponseData.GetErrorResponse<PurchaseProductResponseData>(
                    Errors.NotAvailable,
                    "Store system not initialized"
                );
            }

            return await UseCase.Purchase(itemId);
        }

        // public string test;

        // [Button]
        // public void BuyCheck()
        // {
        //     Buy().Forget();
        // }

        // async UniTask Buy()
        // {
        //     var result = await PurchaseStoreItem(test);

        //     if (!result.Success)
        //     {
        //         Debug.LogError(result.Error);
        //         return;
        //     }

        //     Debug.Log("Item purchased!");
        // }
    }
}