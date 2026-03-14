namespace ConditionEngine.Domain
{
    /// <summary>
    /// Interface cho tất cả Condition
    /// Mỗi Condition phải có khả năng tự evaluate
    /// </summary>
    public interface ICondition
    {
        /// <summary>
        /// Kiểm tra condition có pass hay không
        /// </summary>
        bool Evaluate(IConditionContext context);
    }
}