using Cysharp.Threading.Tasks;

namespace StoreSystem.Domain
{
    /// <summary>
    /// Item miễn phí.
    /// </summary>
    public class FreePriceStrategy : IPriceStrategy
    {
        public PurchaseProductResponseData ValidatePayment()
        {
            return ResponseData.GetSuccessResponse<PurchaseProductResponseData>();
        }

        public UniTask<PurchaseProductResponseData> ExecutePayment()
        {
            var result = ResponseData.GetSuccessResponse<PurchaseProductResponseData>();
            return UniTask.FromResult(result);
        }
    }
}