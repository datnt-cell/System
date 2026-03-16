namespace ConditionEngine.Domain
{
    public class RewardedAdsWatchedCondition : ConditionBase
    {
        private readonly int min;
        private readonly int max;

        public RewardedAdsWatchedCondition(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public override bool Evaluate(IConditionContext context)
        {
            var value = context.RewardedAdsWatched;

            if (value < min)
                return false;

            if (max > 0 && value > max)
                return false;

            return true;
        }
    }
}