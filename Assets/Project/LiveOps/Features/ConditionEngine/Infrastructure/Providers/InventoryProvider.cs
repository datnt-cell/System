namespace ConditionEngine.Infrastructure
{
    /// <summary>
    /// Provider inventory riêng
    /// Có thể kết nối với ItemSystem
    /// </summary>
    public class InventoryProvider
    {
        public bool HasItem(string itemId)
        {
            return false;
        }

        public int GetCurrency(string currencyId)
        {
            return 0;
        }
    }
}