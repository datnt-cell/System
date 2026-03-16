namespace ConditionEngine.Domain
{
    /// <summary>
    /// Condition kiểm tra tiến độ event
    /// Ví dụ: event stage hoặc milestone
    /// </summary>
    public class EventProgressCondition : ICondition
    {
        private readonly string _eventId;
        private readonly int _min;
        private readonly int _max;

        public EventProgressCondition(string eventId, int min, int max)
        {
            _eventId = eventId;
            _min = min;
            _max = max;
        }

        public bool Evaluate(IConditionContext context)
        {
            int progress = context.GetEventProgress(_eventId);

            return progress >= _min && progress <= _max;
        }
    }
}