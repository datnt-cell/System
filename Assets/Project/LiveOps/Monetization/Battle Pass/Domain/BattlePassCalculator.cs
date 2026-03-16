using System.Collections.Generic;

namespace BattlePassModule.Domain
{
    public static class BattlePassCalculator
    {
        public static int CalculateLevel(int xp, IReadOnlyList<BattlePassLevel> levels)
        {
            int level = 0;

            for (int i = 0; i < levels.Count; i++)
            {
                if (xp >= levels[i].RequiredXP)
                    level = levels[i].Level;
                else
                    break;
            }

            return level;
        }
    }
}