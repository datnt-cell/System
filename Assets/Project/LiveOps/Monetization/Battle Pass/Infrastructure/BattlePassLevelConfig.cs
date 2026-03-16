using System;
using Sirenix.OdinInspector;

namespace BattlePassModule.Infrastructure.Config
{
    [Serializable]
    public class BattlePassLevelConfig
    {
        // =========================
        // LEVEL
        // =========================

        [VerticalGroup("Level")]
        [TableColumnWidth(80)]
        [ReadOnly]
        public int Level;

        [VerticalGroup("Level")]
        [TableColumnWidth(100)]
        public int RequiredXP;

        // =========================
        // FREE
        // =========================

        [VerticalGroup("Free")]
        [InlineProperty]
        [LabelText("Free")]
        public BattlePassRewardConfig FreeReward;

        // =========================
        // PREMIUM
        // =========================

        [VerticalGroup("Premium")]
        [InlineProperty]
        [LabelText("Premium")]
        public BattlePassRewardConfig PremiumReward;
    }
}