using System.Collections.Generic;
using Game.Domain.Tasks;

namespace Game.Application.Tasks
{
    /// <summary>
    /// Lưu trạng thái runtime của tất cả Task
    /// </summary>
    public class TaskState
    {
        private readonly Dictionary<TaskId, TaskAggregateRoot> _tasks = new();

        public void AddTask(TaskAggregateRoot task) => _tasks[task.Id] = task;

        public TaskAggregateRoot GetTask(TaskId id) => _tasks.TryGetValue(id, out var task) ? task : null;

        public IEnumerable<TaskAggregateRoot> GetAllTasks() => _tasks.Values;
    }
}