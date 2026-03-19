using Game.Domain.Tasks;
using Game.Application.Tasks.Repositories;
using Game.Application.Tasks.UseCases;
using R3;

namespace Game.Application.Tasks
{
    /// <summary>
    /// TaskService: giống CurrencyService
    /// - Gọi UseCases
    /// - Publish TaskEvent instance-based
    /// - Lưu trạng thái qua repository
    /// </summary>
    public class TaskService
    {
        private readonly TaskState _state;
        private readonly ITaskRepository _repository;
        private readonly TaskEvents _events;

        private readonly ActivateTaskUseCase _activateUseCase;
        private readonly ProgressTaskUseCase _progressUseCase;
        private readonly ClaimRewardUseCase _claimUseCase;

        public TaskService(TaskState state, ITaskRepository repository, TaskEvents events)
        {
            _state = state;
            _repository = repository;
            _events = events;

            _repository.Load(_state);

            _activateUseCase = new ActivateTaskUseCase(_repository);
            _progressUseCase = new ProgressTaskUseCase(_repository);
            _claimUseCase = new ClaimRewardUseCase(_repository);
        }

        public TaskResponse ActivateTask(TaskId id, string source = "")
        {
            var task = _state.GetTask(id);
            if (task == null)
            {
                var evtFail = TaskEvent.CreateFail(id, source, "Task không tồn tại");
                _events.Publish(evtFail);
                return TaskResponse.CreateError("Task không tồn tại");
            }

            try
            {
                _activateUseCase.Execute(id);

                var evt = TaskEvent.CreateSuccess(
                    id,
                    TaskEventType.Activated,
                    task.Progress.Current,
                    task.Definition.Goal.ToString(),
                    source
                );
                _events.Publish(evt);

                SaveAllTasks();

                return TaskResponse.CreateSuccess(task.Progress.Status, task.Progress.Current);
            }
            catch (TaskDomainException ex)
            {
                var evtFail = TaskEvent.CreateFail(id, source, ex.Message);
                _events.Publish(evtFail);
                return TaskResponse.CreateError(ex.Message);
            }
        }

        public TaskResponse ProgressTask(TaskId id, int amount, string source = "")
        {
            var task = _state.GetTask(id);
            if (task == null)
            {
                var evtFail = TaskEvent.CreateFail(id, source, "Task không tồn tại");
                _events.Publish(evtFail);
                return TaskResponse.CreateError("Task không tồn tại");
            }

            try
            {
                _progressUseCase.Execute(id, amount);

                var evt = TaskEvent.CreateSuccess(
                    id,
                    TaskEventType.Progressed,
                    task.Progress.Current,
                    task.Definition.Goal.ToString(),
                    source
                );
                _events.Publish(evt);

                SaveAllTasks();

                return TaskResponse.CreateSuccess(task.Progress.Status, task.Progress.Current);
            }
            catch (TaskDomainException ex)
            {
                var evtFail = TaskEvent.CreateFail(id, source, ex.Message);
                _events.Publish(evtFail);
                return TaskResponse.CreateError(ex.Message);
            }
        }

        public TaskResponse ClaimReward(TaskId id, string source = "")
        {
            var task = _state.GetTask(id);
            if (task == null)
            {
                var evtFail = TaskEvent.CreateFail(id, source, "Task không tồn tại");
                _events.Publish(evtFail);
                return TaskResponse.CreateError("Task không tồn tại");
            }

            try
            {
                _claimUseCase.Execute(id);

                var evt = TaskEvent.CreateSuccess(
                    id,
                    TaskEventType.RewardClaimed,
                    task.Progress.Current,
                    task.Definition.Goal.ToString(),
                    source
                );
                _events.Publish(evt);

                SaveAllTasks();

                return TaskResponse.CreateSuccess(task.Progress.Status, task.Progress.Current);
            }
            catch (TaskDomainException ex)
            {
                var evtFail = TaskEvent.CreateFail(id, source, ex.Message);
                _events.Publish(evtFail);
                return TaskResponse.CreateError(ex.Message);
            }
        }

        public TaskAggregateRoot GetTask(TaskId id) => _state.GetTask(id);

        private void SaveAllTasks()
        {
            foreach (var t in _state.GetAllTasks())
            {
                _repository.Save(t);
            }
        }
    }
}