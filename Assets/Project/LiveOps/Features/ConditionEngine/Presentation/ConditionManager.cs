using System.Collections.Generic;
using UnityEngine;
using ConditionEngine.Application;
using ConditionEngine.Domain;
using ConditionEngine.Infrastructure;
using ConditionEngine.Presentation;

namespace ConditionEngine
{
    /// <summary>
    /// Manager trung tâm để gameplay check condition
    /// </summary>
    public class ConditionManager : MonoBehaviour
    {
        private ConditionService _service;

        // cache condition theo Id
        private readonly Dictionary<string, ICondition> _conditions = new();

        // =========================
        // INIT
        // =========================

        public void Initialize()
        {
            var installer = new ConditionInstaller();
            _service = installer.Install();

            LoadConditions();
        }

        private void LoadConditions()
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

            Debug.Log($"[ConditionManager] Loaded {_conditions.Count} conditions");
        }

        // =========================
        // CHECK SINGLE
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

            return _service.Evaluate(condition);
        }

        // =========================
        // CHECK ALL (AND)
        // =========================

        public bool EvaluateAll(IEnumerable<string> ids)
        {
            foreach (var id in ids)
            {
                if (!Evaluate(id))
                    return false;
            }

            return true;
        }

        // =========================
        // CHECK ANY (OR)
        // =========================

        public bool EvaluateAny(IEnumerable<string> ids)
        {
            foreach (var id in ids)
            {
                if (Evaluate(id))
                    return true;
            }

            return false;
        }

        // =========================
        // RELOAD
        // =========================

        public void Reload()
        {
            LoadConditions();
        }
    }
}