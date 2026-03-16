using CurrencySystem.Domain;

namespace CurrencySystem.Application
{
    public enum CurrencyEventType
    {
        Added,
        Spent
    }

    public class CurrencyEvent
    {
        public CurrencyEventType Type;
        public CurrencyId CurrencyId;
        public int Amount;
        public string Source;

        public static CurrencyEvent Add(CurrencyId id, int amount, string source)
        {
            return new CurrencyEvent
            {
                Type = CurrencyEventType.Added,
                CurrencyId = id,
                Amount = amount,
                Source = source
            };
        }

        public static CurrencyEvent Spend(CurrencyId id, int amount, string source)
        {
            return new CurrencyEvent
            {
                Type = CurrencyEventType.Spent,
                CurrencyId = id,
                Amount = amount,
                Source = source
            };
        }
    }
}