using System;
using System.Collections.Generic;

namespace Game.Domain.Tasks
{
    public class TaskDefinition
    {
        public string Id { get; private set; }
        public TaskGoal Goal { get; private set; }
        public List<CurrencyRewardData> Rewards { get; private set; }

        public TaskDefinition(string id, TaskGoalType goalType, int goalAmount, List<CurrencyRewardData> rewards)
        {
            Id = id;
            Goal = CreateGoal(goalType, goalAmount);
            Rewards = rewards;
        }

        private TaskGoal CreateGoal(TaskGoalType type, int targetAmount)
        {
            return type switch
            {
                TaskGoalType.CollectItem => new CollectItemGoal(targetAmount),
                TaskGoalType.CompleteLevel => new CompleteLevelGoal(targetAmount),
                TaskGoalType.LevelStreak => new LevelStreakGoal(targetAmount),
                _ => throw new NotImplementedException($"Goal type {type} chưa implement")
            };
        }
    }
}