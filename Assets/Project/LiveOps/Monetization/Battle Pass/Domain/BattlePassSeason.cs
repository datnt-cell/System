using System;
using System.Collections.Generic;

namespace BattlePassModule.Domain
{
    public class BattlePassSeason
    {
        public string Id;

        public DateTime StartTime;
        public DateTime EndTime;

        public IReadOnlyList<BattlePassLevel> Levels;

        public BattlePassSeason(
            string id,
            DateTime startTime,
            DateTime endTime,
            IReadOnlyList<BattlePassLevel> levels)
        {
            Id = id;
            StartTime = startTime;
            EndTime = endTime;
            Levels = levels;
        }

        public bool IsActive(DateTime now)
        {
            return now >= StartTime && now <= EndTime;
        }

        public int MaxLevel => Levels.Count;
    }
}