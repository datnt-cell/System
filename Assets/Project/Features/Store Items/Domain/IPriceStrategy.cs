namespace StoreSystem.Domain
{
    /// <summary>
    /// Strategy xử lý logic thanh toán.
    /// </summary>
    public interface IPriceStrategy
    {
        bool CanPay();
        void Pay();
    }
}