namespace ConditionEngine.Domain
{
    /// <summary>
    /// Condition kiểm tra level player
    /// </summary>
    public class PlayerLevelCondition : ConditionBase
    {
        public int MinLevel;

        public int MaxLevel;

        public PlayerLevelCondition(int minLevel, int maxLevel = int.MaxValue)
        {
            MinLevel = minLevel;
            MaxLevel = maxLevel;
        }

        public override bool Evaluate(IConditionContext context)
        {
            int level = context.PlayerLevel;

            return level >= MinLevel && level <= MaxLevel;
        }
    }
}