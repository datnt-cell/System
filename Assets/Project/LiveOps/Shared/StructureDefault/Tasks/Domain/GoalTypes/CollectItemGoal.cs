namespace Game.Domain.Tasks
{
    /// <summary>
    /// Goal thu thập item
    /// </summary>
    public class CollectItemGoal : TaskGoal
    {
        public CollectItemGoal(int targetAmount) { TargetAmount = targetAmount; }
        public override bool IsCompleted(TaskProgress progress) => progress.Current >= TargetAmount;
    }
}