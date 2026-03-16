namespace GameEventModule.Application
{
    public interface ICurrencyService
    {
        void AddCurrency(string currencyId, int amount);
    }
}