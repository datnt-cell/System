using Game.Domain.Tasks;
using Game.Application.Tasks.Repositories;

namespace Game.Application.Tasks.UseCases
{
    public class ProgressTaskUseCase
    {
        private readonly ITaskRepository _repository;
        public ProgressTaskUseCase(ITaskRepository repository) => _repository = repository;

        public void Execute(TaskId taskId, int amount)
        {
            var task = _repository.GetTask(taskId);
            task.UpdateProgress(amount);
            _repository.Save(task);
        }
    }
}