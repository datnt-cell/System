using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UniLabs.Time;

namespace BattlePassModule.Infrastructure.Config
{
    [Serializable]
    public class BattlePassSeasonConfig
    {
        // =========================
        // LEFT PANEL
        // =========================

        [HorizontalGroup("Root", 0.30f)]
        [BoxGroup("Root/Season")]
        [ReadOnly]
        public string Id;

        [HorizontalGroup("Root", 0.30f)]
        [BoxGroup("Root/Season")]
        public string DisplayName;

        [HorizontalGroup("Root", 0.30f)]
        [BoxGroup("Root/Season")]
        public UDateTime StartTime;

        [HorizontalGroup("Root", 0.30f)]
        [BoxGroup("Root/Season")]
        public UDateTime EndTime;

        // =========================
        // RIGHT PANEL
        // =========================

        [HorizontalGroup("Root", 0.70f)]
        [BoxGroup("Root/Levels")]
        [TableList(AlwaysExpanded = true)]
        [OnCollectionChanged(nameof(OnLevelsChanged))]
        public List<BattlePassLevelConfig> Levels = new();

        // =========================
        // AUTO LEVEL
        // =========================

        private void OnLevelsChanged(CollectionChangeInfo info)
        {
            RefreshLevels();
        }

        private void RefreshLevels()
        {
            for (int i = 0; i < Levels.Count; i++)
            {
                Levels[i].Level = i + 1;
            }
        }
    }
}