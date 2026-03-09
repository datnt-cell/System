
using System.Collections.Generic;

namespace CurrencySystem.Domain
{
    public class CurrencyBundle
    {
        /// <summary>
        /// Id của bundle (dùng cho analytics / config)
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Danh sách phần thưởng currency
        /// </summary>
        public IReadOnlyList<CurrencyReward> Rewards { get; }

        public CurrencyBundle(
            string id,
            IReadOnlyList<CurrencyReward> rewards)
        {
            Id = id;
            Rewards = rewards;
        }
    }
}