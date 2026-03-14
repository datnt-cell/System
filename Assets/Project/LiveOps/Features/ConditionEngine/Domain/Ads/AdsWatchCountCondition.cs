namespace ConditionEngine.Domain
{
    /// <summary>
    /// Condition kiểm tra số lần player xem quảng cáo
    /// </summary>
    public class AdsWatchCountCondition : ConditionBase
    {
        private readonly int _min;
        private readonly int _max;

        public AdsWatchCountCondition(int min, int max)
        {
            _min = min;
            _max = max;
        }

        public override bool Evaluate(IConditionContext context)
        {
            int ads = context.AdsWatchCount;

            if (ads < _min)
                return false;

            if (_max > 0 && ads > _max)
                return false;

            return true;
        }
    }
}