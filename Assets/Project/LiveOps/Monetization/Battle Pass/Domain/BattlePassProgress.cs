using System.Collections.Generic;

namespace BattlePassModule.Domain
{
    public class BattlePassProgress
    {
        public string SeasonId;

        public int XP;
        public int CurrentLevel;

        public bool HasPremium;

        public HashSet<int> ClaimedFreeRewards = new();
        public HashSet<int> ClaimedPremiumRewards = new();

        public BattlePassProgress(string seasonId)
        {
            SeasonId = seasonId;
        }

        public bool IsFreeRewardClaimed(int level)
        {
            return ClaimedFreeRewards.Contains(level);
        }

        public bool IsPremiumRewardClaimed(int level)
        {
            return ClaimedPremiumRewards.Contains(level);
        }

        public void ClaimFreeReward(int level)
        {
            ClaimedFreeRewards.Add(level);
        }

        public void ClaimPremiumReward(int level)
        {
            ClaimedPremiumRewards.Add(level);
        }
    }
}