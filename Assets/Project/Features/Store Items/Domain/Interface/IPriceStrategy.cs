using Cysharp.Threading.Tasks;
using IAPModule.Domain.Entities;

namespace StoreSystem.Domain
{
    /// <summary>
    /// Strategy xử lý logic thanh toán.
    /// </summary>
    public interface IPriceStrategy
    {
        bool CanPay();
        UniTask<bool> Pay();
    }
}