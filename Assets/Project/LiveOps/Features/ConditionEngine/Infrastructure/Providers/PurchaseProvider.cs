namespace ConditionEngine.Infrastructure
{
    /// <summary>
    /// Provider cung cấp dữ liệu IAP
    /// </summary>
    public class PurchaseProvider
    {
        public float TotalSpend { get; set; }

        public bool HasPurchased(string productId)
        {
            // TODO: kết nối hệ thống IAP thật
            return false;
        }
    }
}