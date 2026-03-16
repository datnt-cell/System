using ConditionEngine.Domain;

namespace ConditionEngine.Application
{
    public class ConditionEvent
    {
        public ConditionEventType Type;

        public string ConditionId;

        public bool Result;

        public ICondition Condition;

        public static ConditionEvent Evaluated(string id, bool result)
        {
            return new ConditionEvent
            {
                Type = ConditionEventType.Evaluated,
                ConditionId = id,
                Result = result
            };
        }

        public static ConditionEvent True(string id)
        {
            return new ConditionEvent
            {
                Type = ConditionEventType.ConditionTrue,
                ConditionId = id,
                Result = true
            };
        }

        public static ConditionEvent False(string id)
        {
            return new ConditionEvent
            {
                Type = ConditionEventType.ConditionFalse,
                ConditionId = id,
                Result = false
            };
        }

        public static ConditionEvent Reload()
        {
            return new ConditionEvent
            {
                Type = ConditionEventType.Reloaded
            };
        }
    }
}