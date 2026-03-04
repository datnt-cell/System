
using System.Collections.Generic;

namespace CurrencySystem.Domain
{
    public class CurrencyBundle
    {
        public string Id { get; }
        public IReadOnlyList<CurrencyReward> Rewards { get; }
        public bool AutoOpen { get; }

        public CurrencyBundle(
            string id,
            IReadOnlyList<CurrencyReward> rewards,
            bool autoOpen)
        {
            Id = id;
            Rewards = rewards;
            AutoOpen = autoOpen;
        }
    }
}