namespace ConditionEngine.Domain
{
    public class AdsRevenueTodayCondition : ConditionBase
    {
        private readonly double min;
        private readonly double max;

        public AdsRevenueTodayCondition(double min, double max)
        {
            this.min = min;
            this.max = max;
        }

        public override bool Evaluate(IConditionContext context)
        {
            var value = context.AdsRevenueToday;

            if (value < min)
                return false;

            if (max > 0 && value > max)
                return false;

            return true;
        }
    }
}