using System.Collections.Generic;
using Game.Domain.Tasks;

namespace Game.Application.Tasks.Repositories
{
    /// <summary>
    /// Interface repository cho Task
    /// Application chỉ thao tác qua interface, không biết implement chi tiết
    /// </summary>
    public interface ITaskRepository
    {
        TaskAggregateRoot GetTask(TaskId id);           // Lấy Task theo ID
        IEnumerable<TaskAggregateRoot> GetAllTasks();  // Lấy tất cả Task
        void Save(TaskAggregateRoot task);             // Lưu trạng thái Task
        void Load(TaskState state);                     // Load state từ repo
    }
}