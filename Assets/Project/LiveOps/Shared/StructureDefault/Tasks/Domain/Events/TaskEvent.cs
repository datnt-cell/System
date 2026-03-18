using System;
using R3; // Nếu bạn dùng R3 Subject/Observable
using Game.Domain.Tasks;

namespace Game.Application.Tasks
{
    /// <summary>
    /// Kiểu event riêng cho Task
    /// </summary>
    public enum TaskEventType
    {
        Activated,
        Progressed,
        Completed,
        RewardClaimed,
        Failed
    }

    /// <summary>
    /// Event immutable cho Task
    /// Giống style CurrencyBundleEvent: factory + readonly properties
    /// </summary>
    public class TaskEvent
    {
        public TaskEventType EventType { get; }
        public TaskId TaskId { get; }
        public int Progress { get; }
        public string GoalInfo { get; }
        public string Source { get; }
        public bool Success { get; }
        public string ErrorMessage { get; }

        private TaskEvent(
            TaskEventType eventType,
            TaskId taskId,
            int progress,
            string goalInfo,
            string source,
            bool success,
            string errorMessage = null)
        {
            EventType = eventType;
            TaskId = taskId;
            Progress = progress;
            GoalInfo = goalInfo;
            Source = source;
            Success = success;
            ErrorMessage = errorMessage;
        }

        // Factory event thành công
        public static TaskEvent CreateSuccess(
            TaskId taskId,
            TaskEventType eventType,
            int progress = 0,
            string goalInfo = null,
            string source = "")
        {
            return new TaskEvent(eventType, taskId, progress, goalInfo, source, true);
        }

        // Factory event thất bại
        public static TaskEvent CreateFail(
            TaskId taskId,
            string source,
            string errorMessage)
        {
            return new TaskEvent(TaskEventType.Failed, taskId, 0, null, source, false, errorMessage);
        }

        public override string ToString()
        {
            if (Success)
                return $"[Task {EventType}] Id: {TaskId}, Progress: {Progress}, Goal: {GoalInfo}, Source: {Source}";
            else
                return $"[Task Failed] Id: {TaskId}, Source: {Source}, Error: {ErrorMessage}";
        }
    }

    /// <summary>
    /// Event stream cho Task
    /// Giống CurrencyBundleEvents
    /// </summary>
    public interface ITaskEvents
    {
        Observable<TaskEvent> Stream { get; }
    }

    public class TaskEvents : ITaskEvents
    {
        private readonly Subject<TaskEvent> _events = new();

        public Observable<TaskEvent> Stream => _events.AsObservable();

        public void Publish(TaskEvent evt)
        {
            _events.OnNext(evt);
        }
    }
}