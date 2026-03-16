namespace ConditionEngine.Domain
{
    /// <summary>
    /// Condition kiểm tra tổng tiền user đã spend
    /// Dùng cho:
    /// VIP offer
    /// Whale segmentation
    /// </summary>
    public class TotalSpendCondition : ConditionBase
    {
        public float MinSpend;

        public float MaxSpend;

        public TotalSpendCondition(float minSpend, float maxSpend = float.MaxValue)
        {
            MinSpend = minSpend;
            MaxSpend = maxSpend;
        }

        public override bool Evaluate(IConditionContext context)
        {
            float spend = context.TotalSpend;

            return spend >= MinSpend && spend <= MaxSpend;
        }
    }
}