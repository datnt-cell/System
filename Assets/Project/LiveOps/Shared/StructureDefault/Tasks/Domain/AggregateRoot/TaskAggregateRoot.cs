using System;
using Game.Application.Tasks;

namespace Game.Domain.Tasks
{
    /// <summary>
    /// Entity chính của Task
    /// Chứa TaskId, TaskDefinition, TaskProgress
    /// Domain methods: Activate, UpdateProgress, Complete, ClaimReward
    /// Raise event bằng TaskEvent + instance-based TaskEvents
    /// </summary>
    public class TaskAggregateRoot
    {
        public TaskId Id { get; private set; }
        public TaskDefinition Definition { get; private set; }
        public TaskProgress Progress { get; private set; }

        private readonly TaskEvents _events;

        public TaskAggregateRoot(TaskId id, TaskDefinition definition, TaskEvents events)
        {
            Id = id;
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            Progress = new TaskProgress(TaskStatus.Inactive, 0);
            _events = events ?? throw new ArgumentNullException(nameof(events));
        }

        public void SetProgress(TaskProgress progress)
        {
            Progress = progress ?? throw new ArgumentNullException(nameof(progress));
        }

        public void Activate()
        {
            if (Progress.Status != TaskStatus.Inactive)
                throw new TaskDomainException("Task đã được active");

            Progress.SetStatus(TaskStatus.Active);

            // Publish event
            var evt = TaskEvent.CreateSuccess(
                Id,
             Game.Application.Tasks.TaskEventType.Activated,
                Progress.Current,
                Definition.Goal.GetType().Name,
                "System"
            );
            _events.Publish(evt);
        }

        public void UpdateProgress(int amount)
        {
            if (Progress.Status != TaskStatus.Active)
                throw new TaskDomainException("Task chưa active hoặc đã hoàn thành");

            Progress.Increment(amount);

            // Publish progress event
            var evtProgress = TaskEvent.CreateSuccess(
                Id,
               Game.Application.Tasks.TaskEventType.Progressed,
                Progress.Current,
                Definition.Goal.GetType().Name,
                "System"
            );
            _events.Publish(evtProgress);

            // Complete task nếu đạt goal
            if (Definition.Goal.IsCompleted(Progress))
                Complete();
        }

        private void Complete()
        {
            Progress.SetStatus(TaskStatus.Completed);

            var evtComplete = TaskEvent.CreateSuccess(
                Id,
              Game.Application.Tasks.TaskEventType.Completed,
                Progress.Current,
                Definition.Goal.GetType().Name,
                "System"
            );
            _events.Publish(evtComplete);
        }

        public void ClaimReward()
        {
            if (Progress.Status != TaskStatus.Completed)
                throw new TaskDomainException("Chỉ có thể claim reward khi task đã completed");

            Progress.SetStatus(TaskStatus.RewardClaimed);

            var evtClaim = TaskEvent.CreateSuccess(
                Id,
              Game.Application.Tasks.TaskEventType.RewardClaimed,
                Progress.Current,
                Definition.Goal.GetType().Name,
                "System"
            );
            _events.Publish(evtClaim);
        }
    }
}