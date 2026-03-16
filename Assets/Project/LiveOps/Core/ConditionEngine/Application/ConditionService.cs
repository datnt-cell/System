using System.Collections.Generic;
using UnityEngine;
using ConditionEngine.Domain;
using ConditionEngine.Presentation;

namespace ConditionEngine.Application
{
    /// <summary>
    /// Service trung tâm của Condition Engine
    /// Chứa toàn bộ logic evaluate + cache condition
    /// </summary>
    public class ConditionService
    {
        private readonly ConditionEvaluator _evaluator;
        private readonly IConditionContext _context;
        private readonly ConditionEvents _events;

        private readonly Dictionary<string, ICondition> _conditions = new();

        public IConditionEvents Events => _events;

        public ConditionService(
            ConditionEvaluator evaluator,
            IConditionContext context,
            ConditionEvents events)
        {
            _evaluator = evaluator;
            _context = context;
            _events = events;
        }

        // =========================
        // LOAD CONDITIONS
        // =========================

        public void LoadConditions()
        {
            _conditions.Clear();

            var config = ConditionGlobalConfig.Instance;

            if (config == null)
            {
                Debug.LogError("ConditionGlobalConfig not found");
                return;
            }

            foreach (var entry in config.Nodes)
            {
                if (entry == null || entry.Node == null)
                    continue;

                var condition = entry.Node.Build();

                _conditions[entry.Id] = condition;
            }

            Debug.Log($"[ConditionService] Loaded {_conditions.Count} conditions");

            _events.Publish(ConditionEvent.Reload());
        }

        // =========================
        // SINGLE CONDITION
        // =========================

        public bool Evaluate(string conditionId)
        {
            if (string.IsNullOrEmpty(conditionId))
                return true;

            if (!_conditions.TryGetValue(conditionId, out var condition))
            {
                Debug.LogWarning($"Condition not found: {conditionId}");
                return false;
            }

            bool result = _evaluator.Evaluate(condition, _context);

            PublishEvaluateEvent(conditionId, result);

            return result;
        }

        // =========================
        // SAFE EVALUATE
        // =========================

        public bool SafeEvaluate(string conditionId)
        {
            try
            {
                return Evaluate(conditionId);
            }
            catch
            {
                return false;
            }
        }

        // =========================
        // GROUP
        // =========================

        public bool Evaluate(ConditionGroup group)
        {
            if (group == null)
                return true;

            return _evaluator.EvaluateGroup(group, _context);
        }

        // =========================
        // MULTI CONDITIONS
        // =========================

        public bool EvaluateAll(IEnumerable<string> ids)
        {
            if (ids == null)
                return true;

            foreach (var id in ids)
            {
                if (!Evaluate(id))
                    return false;
            }

            return true;
        }

        public bool EvaluateAny(IEnumerable<string> ids)
        {
            if (ids == null)
                return false;

            foreach (var id in ids)
            {
                if (Evaluate(id))
                    return true;
            }

            return false;
        }

        // =========================
        // INTERNAL
        // =========================

        private void PublishEvaluateEvent(string id, bool result)
        {
            _events.Publish(ConditionEvent.Evaluated(id, result));

            if (result)
                _events.Publish(ConditionEvent.True(id));
            else
                _events.Publish(ConditionEvent.False(id));
        }

        // =========================
        // RELOAD
        // =========================

        public void Reload()
        {
            LoadConditions();
        }

        // =========================
        // GETTERS
        // =========================

        public bool HasCondition(string id)
        {
            return _conditions.ContainsKey(id);
        }

        public ICondition GetCondition(string id)
        {
            if (_conditions.TryGetValue(id, out var condition))
                return condition;

            return null;
        }

        public int GetConditionCount()
        {
            return _conditions.Count;
        }

        public IConditionContext GetContext()
        {
            return _context;
        }
    }
}