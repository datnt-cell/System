using Game.Domain.Tasks;
using Game.Application.Tasks.Repositories;

namespace Game.Application.Tasks.UseCases
{
    public class ActivateTaskUseCase
    {
        private readonly ITaskRepository _repository;
        public ActivateTaskUseCase(ITaskRepository repository) => _repository = repository;

        public void Execute(TaskId taskId)
        {
            var task = _repository.GetTask(taskId);
            task.Activate();
            _repository.Save(task);
        }
    }
}