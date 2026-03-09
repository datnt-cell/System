namespace CurrencySystem.Domain
{
    public readonly struct CurrencyReward
    {
        /// <summary>
        /// Loại currency
        /// </summary>
        public CurrencyId Id { get; }

        /// <summary>
        /// Số lượng
        /// </summary>
        public int Amount { get; }

        public CurrencyReward(CurrencyId id, int amount)
        {
            Id = id;
            Amount = amount;
        }
    }
}