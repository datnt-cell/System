using CurrencySystem.Application;
using GameEventModule.Application;

public class CurrencyServiceAdapter : ICurrencyService
{
    private readonly CurrencyService currencyService;

    public CurrencyServiceAdapter(CurrencyService currencyService)
    {
        this.currencyService = currencyService;
    }

    public void AddCurrency(string currencyId, int amount)
    {
        if (string.IsNullOrEmpty(currencyId) || amount <= 0)
            return;

      //  currencyService.AddCurrency(new CurrencySystem.Domain.CurrencyId(currencyId), amount);
    }
}