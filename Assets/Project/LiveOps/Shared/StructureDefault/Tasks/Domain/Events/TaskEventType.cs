namespace Game.Domain.Tasks
{
    /// <summary>
    /// Kiểu event của Task
    /// </summary>
    public enum TaskEventType
    {
        Activated,
        Progressed,
        Completed,
        RewardClaimed,
        Failed
    }
}