using System.Collections.Generic;
using StoreSystem.Domain;

namespace StoreSystem.Application
{
    /// <summary>
    /// UseCase cho việc mua StoreItem.
    /// </summary>
    public class StoreItemUseCase
    {
        private readonly Dictionary<string, StoreItem> _items = new();

        public StoreItemUseCase(List<StoreItem> items)
        {
            foreach (var item in items)
            {
                _items[item.Id] = item;
            }
        }

        /// <summary>
        /// Lấy toàn bộ item
        /// </summary>
        public IReadOnlyCollection<StoreItem> GetAll()
        {
            return _items.Values;
        }

        /// <summary>
        /// Lấy item theo id
        /// </summary>
        public StoreItem Get(string id)
        {
            _items.TryGetValue(id, out var item);
            return item;
        }

        /// <summary>
        /// Thử mua item
        /// </summary>
        public bool TryPurchase(string id)
        {
            var item = Get(id);

            if (item == null)
                return false;

            return item.TryPurchase();
        }
    }
}