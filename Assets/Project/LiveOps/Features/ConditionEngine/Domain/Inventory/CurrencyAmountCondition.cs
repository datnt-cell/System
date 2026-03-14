namespace ConditionEngine.Domain
{
    /// <summary>
    /// Condition kiểm tra số lượng currency
    /// Ví dụ:
    /// cần >= 100 gold
    /// </summary>
    public class CurrencyAmountCondition : ConditionBase
    {
        public string CurrencyId;

        public int MinAmount;

        public int MaxAmount;

        public CurrencyAmountCondition(string currencyId, int minAmount, int maxAmount = int.MaxValue)
        {
            CurrencyId = currencyId;
            MinAmount = minAmount;
            MaxAmount = maxAmount;
        }

        public override bool Evaluate(IConditionContext context)
        {
            int amount = context.GetCurrency(CurrencyId);

            return amount >= MinAmount && amount <= MaxAmount;
        }
    }
}