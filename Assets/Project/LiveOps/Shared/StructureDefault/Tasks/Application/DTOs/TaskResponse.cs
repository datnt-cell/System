using Game.Domain.Tasks;

namespace Game.Application.Tasks
{
    /// <summary>
    /// Response chi tiết cho TaskService, giống CurrencyResponse
    /// </summary>
    public class TaskResponse
    {
        public bool Success { get; private set; }
        public string ErrorMessage { get; private set; }
        public TaskStatus Status { get; private set; }
        public int Progress { get; private set; }

        public static TaskResponse CreateSuccess(TaskStatus status, int progress)
        {
            return new TaskResponse
            {
                Success = true,
                Status = status,
                Progress = progress
            };
        }

        public static TaskResponse CreateError(string error)
        {
            return new TaskResponse
            {
                Success = false,
                ErrorMessage = error
            };
        }
    }
}