using StoreSystem.Domain;

namespace StoreSystem.Application
{
    /// <summary>
    /// UseCase cho việc mua StoreItem.
    /// </summary>
    public class StoreItemUseCase
    {
        private readonly StoreItem _storeItem;

        public StoreItemUseCase(StoreItem storeItem)
        {
            _storeItem = storeItem;
        }

        public bool TryPurchase()
        {
            return _storeItem.TryPurchase();
        }
    }
}