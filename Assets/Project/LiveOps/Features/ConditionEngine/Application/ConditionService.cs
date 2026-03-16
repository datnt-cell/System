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
            if (condition == null)
                return true;

            return _evaluator.Evaluate(condition, _context);
        }

        /// <summary>
        /// Safe evaluate (không crash)
        /// </summary>
        public bool SafeEvaluate(ICondition condition)
        {
            try
            {
                return Evaluate(condition);
            }
            catch
            {
                return false;
            }
        }

        // =========================
        // GROUP
        // =========================

        /// <summary>
        /// Evaluate condition group
        /// </summary>
        public bool Evaluate(ConditionGroup group)
        {
            if (group == null)
                return true;

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
            if (conditions == null)
                return true;

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
            if (conditions == null)
                return false;

            foreach (var condition in conditions)
            {
                if (_evaluator.Evaluate(condition, _context))
                    return true;
            }

            return false;
        }

        // =========================
        // BATCH EVALUATE
        // =========================

        /// <summary>
        /// Evaluate list và trả về kết quả từng condition
        /// </summary>
        public Dictionary<ICondition, bool> EvaluateBatch(IEnumerable<ICondition> conditions)
        {
            var results = new Dictionary<ICondition, bool>();

            if (conditions == null)
                return results;

            foreach (var condition in conditions)
            {
                results[condition] = _evaluator.Evaluate(condition, _context);
            }

            return results;
        }

        // =========================
        // CONTEXT
        // =========================

        public IConditionContext GetContext()
        {
            return _context;
        }
    }
}