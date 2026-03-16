using Cysharp.Threading.Tasks;
using CurrencySystem.Application;
using CurrencySystem.Domain;

[System.Serializable]
public class CurrencyPrice
{
    public CurrencyId CurrencyId;
    public int Amount;

    public CurrencyPrice(CurrencyId currencyId, int amount)
    {
        CurrencyId = currencyId;
        Amount = amount;
    }

    public CurrencyPrice(CurrencyRewardConfig currencyReward)
    {
        CurrencyId = new CurrencyId(currencyReward.CurrencyId);
        Amount = currencyReward.Amount;
    }
}