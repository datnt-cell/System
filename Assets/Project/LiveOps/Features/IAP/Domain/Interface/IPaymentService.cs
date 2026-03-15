namespace IAPModule.Application.Interfaces
{
    public interface IPaymentService
    {
        void RegisterPurchase(float price, string currency);

        float GetTotalSpend();

        int GetPurchaseCount();

        bool HasPurchased(string productId);

        bool HasAnyPurchase();
    }
}