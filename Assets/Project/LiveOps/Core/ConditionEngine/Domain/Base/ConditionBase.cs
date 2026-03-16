namespace ConditionEngine.Domain
{
    /// <summary>
    /// Base class cho tất cả Condition
    /// Có thể extend thêm debug sau này
    /// </summary>
    public abstract class ConditionBase : ICondition
    {
        /// <summary>
        /// Hàm evaluate chính
        /// </summary>
        public abstract bool Evaluate(IConditionContext context);
    }
}