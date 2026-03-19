namespace Game.Domain.Tasks
{
    /// <summary>
    /// Goal duy trì streak level
    /// </summary>
    public class LevelStreakGoal : TaskGoal
    {
        public LevelStreakGoal(int targetAmount) { TargetAmount = targetAmount; }
        public override bool IsCompleted(TaskProgress progress) => progress.Current >= TargetAmount;
    }
}