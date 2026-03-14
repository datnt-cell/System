using UnityEngine;
using ConditionEngine.Application;
using ConditionEngine.Domain;

namespace ConditionEngine.Presentation
{
    /// <summary>
    /// Debug condition trong runtime
    /// </summary>
    public class ConditionDebugView : MonoBehaviour
    {
        [SerializeField]
        private ConditionConfig config;

        private ConditionService _service;

        private ICondition _runtimeCondition;

        public void Init(ConditionService service)
        {
            _service = service;

            // build condition runtime
            if (config != null)
                _runtimeCondition = config.Build();
        }

        [ContextMenu("Check Condition")]
        public void Check()
        {
            if (_runtimeCondition == null)
            {
                Debug.LogWarning("Condition chưa được build");
                return;
            }

            bool result = _service.Check(_runtimeCondition);

            Debug.Log($"Condition Result: {result}");
        }
    }
}