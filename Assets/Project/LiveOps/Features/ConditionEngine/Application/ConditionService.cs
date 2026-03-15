using System.Collections.Generic;
using ConditionEngine.Domain;

namespace ConditionEngine.Application
{
    /// <summary>
    /// Service dùng để evaluate condition trong gameplay / LiveOps
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

        // =========================
        // SINGLE CONDITION
        // =========================

        /// <summary>
        /// Evaluate một condition
        /// </summary>
        public bool Evaluate(ICondition condition)
        {
            return _evaluator.Evaluate(condition, _context);
        }

        // =========================
        // GROUP
        // =========================

        /// <summary>
        /// Evaluate condition group
        /// </summary>
        public bool Evaluate(ConditionGroup group)
        {
            return _evaluator.EvaluateGroup(group, _context);
        }

        // =========================
        // MULTI CONDITIONS
        // =========================

        /// <summary>
        /// Tất cả condition phải đúng
        /// </summary>
        public bool EvaluateAll(IEnumerable<ICondition> conditions)
        {
            foreach (var condition in conditions)
            {
                if (!_evaluator.Evaluate(condition, _context))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Chỉ cần một condition đúng
        /// </summary>
        public bool EvaluateAny(IEnumerable<ICondition> conditions)
        {
            foreach (var condition in conditions)
            {
                if (_evaluator.Evaluate(condition, _context))
                    return true;
            }

            return false;
        }
    }
}