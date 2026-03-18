using System;

namespace Game.Domain.Tasks
{
    /// <summary>
    /// Reward của Task
    /// </summary>
       [System.Serializable]
    public class TaskReward
    {
        public string RewardId { get; private set; }
        public int Quantity { get; private set; }

        public TaskReward(string rewardId, int quantity)
        {
            if (string.IsNullOrEmpty(rewardId))
                throw new ArgumentException("RewardId không được rỗng");
            if (quantity <= 0)
                throw new ArgumentException("Quantity phải > 0");

            RewardId = rewardId;
            Quantity = quantity;
        }
    }
}