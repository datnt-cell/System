using ConditionEngine.Domain;

namespace ConditionEngine.Application
{
    /// <summary>
    /// Service để gameplay check condition
    /// </summary>
    public class ConditionService
    {
        private readonly ConditionEvaluator _evaluator;
        private readonly IConditionContext _context;

        public ConditionService(
            ConditionEvaluator evaluator,
            IConditionContext context)
        {
            _evaluator = evaluator;
            _context = context;
        }

        /// <summary>
        /// Kiểm tra một condition
        /// </summary>
        public bool Check(ICondition condition)
        {
            return _evaluator.Evaluate(condition, _context);
        }

        /// <summary>
        /// Kiểm tra group condition
        /// </summary>
        public bool CheckGroup(ConditionGroup group)
        {
            return _evaluator.EvaluateGroup(group, _context);
        }
    }
}