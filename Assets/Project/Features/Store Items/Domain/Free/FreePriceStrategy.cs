using Cysharp.Threading.Tasks;
using IAPModule.Domain.Entities;

namespace StoreSystem.Domain
{
    /// <summary>
    /// Item miễn phí.
    /// </summary>
    public class FreePriceStrategy : IPriceStrategy
    {
        public bool CanPay() => true;

        public async UniTask<bool> Pay()
        {
            return true;
        }
    }
}