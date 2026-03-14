using ConditionEngine.Domain;
using LBG;
using Sirenix.OdinInspector;

namespace ConditionEngine.Presentation
{
    /// <summary>
    /// Base node cho condition config
    /// Node này sẽ convert sang Domain Condition
    /// </summary>
    [System.Serializable]
    public abstract class ConditionNode
    {
        public string Summary => GetSummary();

        protected virtual string GetSummary()
        {
            return GetType().Name;
        }
        

        public abstract ICondition Build();
    }
}