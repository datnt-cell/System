using System.Collections.Generic;
using Game.Domain.Tasks;
using Game.Application.Tasks.Repositories;
using UnityEngine;
using Game.Application.Tasks;

namespace Game.Infrastructure.Tasks.Repositories
{
    /// <summary>
    /// Repository lưu trạng thái Task bằng EasySave3
    /// - Lưu/Load trực tiếp TaskState runtime
    /// - Hỗ trợ filePath tùy chỉnh
    /// </summary>
    public class EasySaveTaskRepository : ITaskRepository
    {
        private readonly string _filePath = "TaskState.es3";


        /// <summary>
        /// Lấy Task theo ID
        /// </summary>
        public TaskAggregateRoot GetTask(TaskId id)
        {
            if (!ES3.KeyExists(_filePath)) return null;

            var savedTasks = ES3.Load<Dictionary<string, TaskProgress>>(_filePath);
            if (!savedTasks.TryGetValue(id.Value, out var progress)) return null;

            // Chú ý: đây chưa tạo TaskAggregateRoot đầy đủ (thiếu Definition)
            // Nên chỉ dùng khi đã có TaskDefinition sẵn trong TaskState
            var task = new TaskAggregateRoot(id, null, new TaskEvents());
            task.SetProgress(progress); // dùng method set, vì Progress có private set
            return task;
        }

        /// <summary>
        /// Lấy tất cả Task
        /// </summary>
        public IEnumerable<TaskAggregateRoot> GetAllTasks()
        {
            if (!ES3.KeyExists(_filePath)) yield break;

            var savedTasks = ES3.Load<Dictionary<string, TaskProgress>>(_filePath);
            foreach (var kv in savedTasks)
            {
                var task = new TaskAggregateRoot(new TaskId(kv.Key), null, new TaskEvents());
                task.SetProgress(kv.Value); // dùng method SetProgress thay vì set trực tiếp
                yield return task;
            }
        }

        /// <summary>
        /// Lưu trạng thái Task
        /// </summary>
        public void Save(TaskAggregateRoot task)
        {
            Dictionary<string, TaskProgress> dict;
            if (ES3.KeyExists(_filePath))
                dict = ES3.Load<Dictionary<string, TaskProgress>>(_filePath);
            else
                dict = new Dictionary<string, TaskProgress>();

            dict[task.Id.Value] = task.Progress;
            ES3.Save(_filePath, dict);

            Debug.Log($"[EasySaveTaskRepository] Saved task {task.Id.Value} to {_filePath}");
        }

        /// <summary>
        /// Load tất cả TaskProgress vào TaskState runtime
        /// </summary>
        public void Load(TaskState state)
        {
            if (!ES3.KeyExists(_filePath)) return;

            var savedTasks = ES3.Load<Dictionary<string, TaskProgress>>(_filePath);

            foreach (var kv in savedTasks)
            {
                var taskId = new TaskId(kv.Key);
                var task = state.GetTask(taskId);
                if (task != null)
                    task.SetProgress(kv.Value);
            }

            Debug.Log($"[EasySaveTaskRepository] Loaded {savedTasks.Count} tasks from {_filePath}");
        }
    }
}