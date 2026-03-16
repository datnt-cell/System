using BattlePassModule.Application;
using BattlePassModule.Domain;
using UnityEngine;

namespace BattlePassModule.Infrastructure.Repository
{
    public class EasySaveBattlePassRepository : IBattlePassRepository
    {
        private const string KEY = "battlepass_progress";

        private const string FILE = "battlepass.es3";

        public BattlePassProgress LoadProgress(string seasonId)
        {
            if (!ES3.KeyExists(KEY, FILE))
                return null;

            var progress = ES3.Load<BattlePassProgress>(KEY, FILE);

            if (progress.SeasonId != seasonId)
                return null;

            return progress;
        }

        public void SaveProgress(BattlePassProgress progress)
        {
            ES3.Save(KEY, progress, FILE);
        }
    }
}