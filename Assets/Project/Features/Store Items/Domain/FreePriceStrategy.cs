namespace StoreSystem.Domain
{
    /// <summary>
    /// Item miễn phí.
    /// </summary>
    public class FreePriceStrategy : IPriceStrategy
    {
        public bool CanPay() => true;
        public void Pay() { }
    }
}