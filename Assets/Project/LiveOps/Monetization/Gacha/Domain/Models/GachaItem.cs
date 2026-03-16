using System;

public enum GachaRewardType
{
    Currency,
    CurrencyBundle
}

namespace GachaSystem.Domain.Models
{
    [Serializable]
    public class GachaItem
    {
        public GachaRewardType RewardType { get; }

        /// <summary>
        /// CurrencyId hoặc BundleId
        /// </summary>
        public string RewardId { get; }

        /// <summary>
        /// Amount chỉ dùng cho Currency
        /// </summary>
        public int Amount { get; }

        public int Weight { get; }

        public GachaItem(
            GachaRewardType rewardType,
            string rewardId,
            int amount,
            int weight)
        {
            if (string.IsNullOrEmpty(rewardId))
                throw new ArgumentException("RewardId cannot be null");

            if (weight <= 0)
                throw new ArgumentException("Weight must be > 0");

            if (rewardType == GachaRewardType.Currency && amount <= 0)
                throw new ArgumentException("Currency reward must have amount > 0");

            RewardType = rewardType;
            RewardId = rewardId;
            Amount = rewardType == GachaRewardType.Currency ? amount : 1;
            Weight = weight;
        }
    }
}