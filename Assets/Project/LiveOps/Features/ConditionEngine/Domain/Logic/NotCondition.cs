namespace ConditionEngine.Domain
{
    /// <summary>
    /// Logic NOT
    /// </summary>
    public class NotCondition : ConditionBase
    {
        private readonly ICondition _condition;

        public NotCondition(ICondition condition)
        {
            _condition = condition;
        }

        public override bool Evaluate(IConditionContext context)
        {
            if (_condition == null)
                return false;

            return !_condition.Evaluate(context);
        }
    }
}