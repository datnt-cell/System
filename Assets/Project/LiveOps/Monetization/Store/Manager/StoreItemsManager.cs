using UnityEngine;
using StoreSystem.Application;
using StoreSystem.Installer;
using StoreSystem.Infrastructure;

namespace StoreSystem.Presentation
{
    /// <summary>
    /// Danh sách các vật phẩm có thể bán trực tiếp trong Shop hoặc thông qua Game Offers.
    /// </summary>
    public class StoreItemsManager : MonoBehaviour
    {
        public StoreService Service { get; private set; }

        public StoreEvents Events { get; private set; }

        private StoreItemUseCase _useCase;

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

            _useCase = result.UseCase;
            Events = result.Events;

            Service = new StoreService(
                _useCase,
                Events
            );
        }
    }
}