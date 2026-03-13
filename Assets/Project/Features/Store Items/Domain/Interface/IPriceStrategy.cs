using Cysharp.Threading.Tasks;

namespace StoreSystem.Domain
{
    /// <summary>
    /// Strategy xử lý logic thanh toán.
    /// </summary>
    public interface IPriceStrategy
    {
        PurchaseProductResponseData ValidatePayment();

        UniTask<PurchaseProductResponseData> ExecutePayment();
    }
}