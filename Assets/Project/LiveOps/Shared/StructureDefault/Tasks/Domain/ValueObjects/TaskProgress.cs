namespace Game.Domain.Tasks
{
    /// <summary>
    /// Lưu trạng thái và tiến trình hiện tại của task
    /// </summary>
    public class TaskProgress
    {
        public TaskStatus Status { get; private set; }
        public int Current { get; private set; }

        public TaskProgress(TaskStatus status, int current)
        {
            Status = status;
            Current = current;
        }

        public void Increment(int amount)
        {
            if (amount <= 0) return;
            Current += amount;
        }

        public void SetStatus(TaskStatus status)
        {
            Status = status;
        }
    }
}