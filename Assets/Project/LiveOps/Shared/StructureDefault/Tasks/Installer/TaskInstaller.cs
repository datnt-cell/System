using UnityEngine;
using Game.Application.Tasks;
using Game.Application.Tasks.Repositories;
using Game.Domain.Tasks;
using Game.Infrastructure.Tasks;
using Game.Infrastructure.Tasks.Repositories;

namespace Game.Installer
{
    /// <summary>
    /// Installer Task module
    /// - Compose dependencies: State, Repository, TaskEvents, Service
    /// - Trả về TaskInstallResult
    /// </summary>
    public class TaskInstaller
    {

        public TaskInstallResult Install()
        {
            // =========================
            // DOMAIN STATE + EVENTS
            // =========================
            var state = new TaskState();
            var taskEvents = new TaskEvents(); // instance-based events

            // =========================
            // REPOSITORY (Infrastructure)
            // =========================
            ITaskRepository repository = new EasySaveTaskRepository();
            repository.Load(state);

            // =========================
            // Khởi tạo TaskAggregateRoot từ TasksGlobalConfig
            // =========================
            foreach (var taskCfg in TasksGlobalConfig.Instance.Tasks)
            {
                var definition = new TaskDefinition(
                    taskCfg.Id,
                    taskCfg.GoalType,
                    taskCfg.GoalAmount,
                    taskCfg.Rewards
                );

                var task = new TaskAggregateRoot(new TaskId(taskCfg.Id), definition, taskEvents);
                state.AddTask(task);
            }

            // =========================
            // APPLICATION SERVICE
            // =========================
            var service = new TaskService(state, repository, taskEvents);

            return new TaskInstallResult(service, state, repository, taskEvents);
        }
    }

    public readonly struct TaskInstallResult
    {
        public TaskService Service { get; }
        public TaskState State { get; }
        public ITaskRepository Repository { get; }
        public TaskEvents Events { get; }

        public TaskInstallResult(
            TaskService service,
            TaskState state,
            ITaskRepository repository,
            TaskEvents events)
        {
            Service = service;
            State = state;
            Repository = repository;
            Events = events;
        }
    }
}