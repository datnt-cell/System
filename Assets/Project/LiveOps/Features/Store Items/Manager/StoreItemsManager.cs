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
        public StoreService Service { get; private set; }

        private StoreItemUseCase UseCase;

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
            Service = new StoreService(UseCase);
        }
    }
}