namespace ConditionEngine.Domain
{
    /// <summary>
    /// Condition kiểm tra tổng play time của player
    /// </summary>
    public class PlayTimeCondition : ICondition
    {
        private readonly int _minMinutes;
        private readonly int _maxMinutes;

        public PlayTimeCondition(int minMinutes, int maxMinutes)
        {
            _minMinutes = minMinutes;
            _maxMinutes = maxMinutes;
        }

        public bool Evaluate(IConditionContext context)
        {
            int playTime = context.TotalPlayTimeMinutes;

            return playTime >= _minMinutes && playTime <= _maxMinutes;
        }
    }
}