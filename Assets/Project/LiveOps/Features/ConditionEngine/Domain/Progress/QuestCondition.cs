namespace ConditionEngine.Domain
{
    /// <summary>
    /// Condition kiểm tra quest đã hoàn thành chưa
    /// </summary>
    public class QuestCondition : ConditionBase
    {
        public string QuestId;

        public QuestCondition(string questId)
        {
            QuestId = questId;
        }

        public override bool Evaluate(IConditionContext context)
        {
            // Domain chỉ định nghĩa logic
            // Context sẽ implement việc check quest
            return context.HasItem(QuestId);
        }
    }
}