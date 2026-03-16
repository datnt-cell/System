using System;

namespace GachaSystem.Domain.Models
{
    public enum GachaRewardType
    {
        Currency,
        CurrencyBundle
    }
}

namespace GachaSystem.Domain.Models
{
    [Serializable]
    public class GachaItem
    {
        public GachaRewardType RewardType;

        public string RewardId;

        public int Amount;

        public int Weight;

        public GachaItem(
            GachaRewardType rewardType,
            string rewardId,
            int amount,
            int weight)
        {
            RewardType = rewardType;
            RewardId = rewardId;
            Amount = amount;
            Weight = weight;
        }
    }
}