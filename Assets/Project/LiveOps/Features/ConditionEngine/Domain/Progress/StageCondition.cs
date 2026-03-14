namespace ConditionEngine.Domain
{
    /// <summary>
    /// Condition kiểm tra stage / level progression
    /// </summary>
    public class StageCondition : ConditionBase
    {
        public int MinStage;

        public int MaxStage;

        public StageCondition(int minStage, int maxStage = int.MaxValue)
        {
            MinStage = minStage;
            MaxStage = maxStage;
        }

        public override bool Evaluate(IConditionContext context)
        {
            int stage = context.Stage;

            return stage >= MinStage && stage <= MaxStage;
        }
    }
}