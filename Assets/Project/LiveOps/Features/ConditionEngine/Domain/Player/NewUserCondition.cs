namespace ConditionEngine.Domain
{
    /// <summary>
    /// Condition kiểm tra player có phải user mới không
    /// </summary>
    public class NewUserCondition : ConditionBase
    {
        public bool RequiredState;

        public NewUserCondition(bool requiredState = true)
        {
            RequiredState = requiredState;
        }

        public override bool Evaluate(IConditionContext context)
        {
            return context.IsNewUser == RequiredState;
        }
    }
}