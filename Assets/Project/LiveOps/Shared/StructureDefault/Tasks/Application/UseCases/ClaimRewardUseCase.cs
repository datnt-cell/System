using Game.Domain.Tasks;
using Game.Application.Tasks.Repositories;

namespace Game.Application.Tasks.UseCases
{
    public class ClaimRewardUseCase
    {
        private readonly ITaskRepository _repository;
        public ClaimRewardUseCase(ITaskRepository repository) => _repository = repository;

        public void Execute(TaskId taskId)
        {
            var task = _repository.GetTask(taskId);
            task.ClaimReward();
            _repository.Save(task);
        }
    }
}