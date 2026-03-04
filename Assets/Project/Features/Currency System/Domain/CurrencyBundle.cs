
using System.Collections.Generic;

namespace CurrencySystem.Domain
{
    public class CurrencyBundle
    {
        public string Id { get; }
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