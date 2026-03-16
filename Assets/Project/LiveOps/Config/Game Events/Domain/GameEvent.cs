using System.Collections.Generic;
using ConditionEngine.Domain;

namespace GameEventModule.Domain
{
    public class GameEvent
    {
        public GameEventId Id { get; }

        public int Priority { get; }

        public ICondition Condition { get; }

        public EventFinishPolicy FinishPolicy { get; }

        public IReadOnlyList<IGameEventAttachment> Attachments { get; }

        public GameEvent(
            GameEventId id,
            int priority,
            ICondition condition,
            EventFinishPolicy finishPolicy,
            IReadOnlyList<IGameEventAttachment> attachments)
        {
            Id = id;
            Priority = priority;
            Condition = condition;
            FinishPolicy = finishPolicy;
            Attachments = attachments;
        }

        public bool CanStart(IConditionContext context)
        {
            if (Condition == null)
                return true;

            return Condition.Evaluate(context);
        }

        public bool HasAttachments()
        {
            return Attachments != null && Attachments.Count > 0;
        }

        public bool IsHigherPriorityThan(GameEvent other)
        {
            return Priority > other.Priority;
        }
    }
}