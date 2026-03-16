namespace ConditionEngine.Domain
{
    /// <summary>
    /// Condition kiểm tra build version của app
    /// Dùng để lock feature theo build
    /// </summary>
    public class AppBuildVersionCondition : ConditionBase
    {
        private readonly int _minVersion;
        private readonly int _maxVersion;

        public AppBuildVersionCondition(int minVersion, int maxVersion)
        {
            _minVersion = minVersion;
            _maxVersion = maxVersion;
        }

        public override bool Evaluate(IConditionContext context)
        {
            int current = context.AppBuildVersion;

            if (_minVersion > 0 && current < _minVersion)
                return false;

            if (_maxVersion > 0 && current > _maxVersion)
                return false;

            return true;
        }
    }
}