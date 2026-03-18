namespace Game.Domain.Tasks
{
    /// <summary>
    /// Abstract class cho mọi loại Goal
    /// </summary>
    public abstract class TaskGoal
    {
        public int TargetAmount { get; protected set; }
        
        /// <summary>
        /// Kiểm tra goal đã hoàn thành chưa
        /// </summary>
        public abstract bool IsCompleted(TaskProgress progress);
    }
}