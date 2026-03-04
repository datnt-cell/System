namespace IAPModule.Domain.Entities
{
    /// <summary>
    /// Kết quả mua hàng ở tầng Domain
    /// Không phụ thuộc SDK
    /// </summary>
    public class PurchaseResult
    {
        public string ProductId { get; }
        public bool IsSuccess { get; }

        public PurchaseResult(string productId, bool isSuccess)
        {
            ProductId = productId;
            IsSuccess = isSuccess;
        }
    }
}