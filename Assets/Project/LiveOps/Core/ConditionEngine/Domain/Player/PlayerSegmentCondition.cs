namespace ConditionEngine.Domain
{
    /// <summary>
    /// Condition kiểm tra player thuộc segment nào
    /// Ví dụ: whale, spender, non_spender
    /// </summary>
    public class PlayerSegmentCondition : ICondition
    {
        private readonly string _segment;

        public PlayerSegmentCondition(string segment)
        {
            _segment = segment;
        }

        public bool Evaluate(IConditionContext context)
        {
            return context.PlayerSegment == _segment;
        }
    }
}