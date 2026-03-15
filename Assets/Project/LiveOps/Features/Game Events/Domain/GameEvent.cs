using ConditionEngine.Domain;

namespace GameEventModule.Domain
{
    /// <summary>
    /// Entity đại diện cho Game Event
    /// </summary>
    public class GameEvent
    {
        /// <summary>
        /// ID event
        /// </summary>
        public GameEventId Id { get; }

        /// <summary>
        /// Priority dùng để sort event
        /// </summary>
        public int Priority { get; }

        /// <summary>
        /// Condition kích hoạt event
        /// </summary>
        public ICondition Condition { get; }

        /// <summary>
        /// Chính sách kết thúc event
        /// </summary>
        public EventFinishPolicy FinishPolicy { get; }

        /// <summary>
        /// Attachment thực thi khi event start
        /// </summary>
        public IGameEventAttachment Attachment { get; }

        public GameEvent(
               GameEventId id,
               int priority,
               ICondition condition,
               EventFinishPolicy finishPolicy,
               IGameEventAttachment attachment)
        {
            Id = id;
            Priority = priority;
            Condition = condition;
            FinishPolicy = finishPolicy;
            Attachment = attachment;
        }

        /// <summary>
        /// Kiểm tra event có thể start hay không
        /// </summary>
        public bool CanStart(IConditionContext context)
        {
            if (Condition == null)
                return true;

            return Condition.Evaluate(context);
        }
    }
}