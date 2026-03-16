using System;

namespace BattlePassModule.Domain
{
    public class BattlePassLevel
    {
        public int Level;
        public int RequiredXP;

        public BattlePassReward FreeReward;
        public BattlePassReward PremiumReward;

        public BattlePassLevel(
            int level,
            int requiredXP,
            BattlePassReward freeReward,
            BattlePassReward premiumReward)
        {
            Level = level;
            RequiredXP = requiredXP;
            FreeReward = freeReward;
            PremiumReward = premiumReward;
        }
    }
}