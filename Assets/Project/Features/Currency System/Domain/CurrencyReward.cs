namespace CurrencySystem.Domain
{
    public readonly struct CurrencyReward
    {
        public CurrencyId Id { get; }
        public int Amount { get; }

        public CurrencyReward(CurrencyId id, int amount)
        {
            Id = id;
            Amount = amount;
        }
    }
}