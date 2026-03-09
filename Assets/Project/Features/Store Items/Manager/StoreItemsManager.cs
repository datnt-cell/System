using UnityEngine;
using StoreSystem.Application;
using StoreSystem.Installer;

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
                currencyManager.CurrencyService,
                currencyManager.BundleUseCase
            );

            var result = installer.Install();

            UseCase = result.UseCase;
        }


        /// <summary>
        /// Mua item
        /// </summary>
        public bool Purchase(string itemId)
        {
            if (UseCase == null)
            {
                Debug.LogError("StoreItemsManager not initialized");
                return false;
            }

            return UseCase.TryPurchase(itemId);
        }
    }
}