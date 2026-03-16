namespace ConditionEngine.Domain
{
    public class TotalAdsRevenueCondition : ConditionBase
    {
        private readonly double min;
        private readonly double max;

        public TotalAdsRevenueCondition(double min, double max)
        {
            this.min = min;
            this.max = max;
        }

        public override bool Evaluate(IConditionContext context)
        {
            var value = context.TotalAdsRevenue;

            if (value < min)
                return false;

            if (max > 0 && value > max)
                return false;

            return true;
        }
    }
}