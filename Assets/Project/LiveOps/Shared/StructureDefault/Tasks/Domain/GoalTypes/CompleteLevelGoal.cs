namespace Game.Domain.Tasks
{
    /// <summary>
    /// Goal hoàn thành level
    /// </summary>
    public class CompleteLevelGoal : TaskGoal
    {
        public CompleteLevelGoal(int targetAmount) { TargetAmount = targetAmount; }
        public override bool IsCompleted(TaskProgress progress) => progress.Current >= TargetAmount;
    }
}