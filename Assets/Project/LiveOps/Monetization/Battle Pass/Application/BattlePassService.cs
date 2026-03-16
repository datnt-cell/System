using System;
using System.Linq;
using BattlePassModule.Domain;
using R3;

namespace BattlePassModule.Application
{
    public class BattlePassService : IDisposable
    {
        private readonly IBattlePassRepository _repository;
        private readonly BattlePassEvents _events;

        private BattlePassSeason _season;
        private BattlePassProgress _progress;

        public BattlePassService(
            IBattlePassRepository repository,
            BattlePassEvents events)
        {
            _repository = repository;
            _events = events;
        }

        public void Initialize(BattlePassSeason season)
        {
            _season = season;

            _progress = _repository.LoadProgress(season.Id);

            if (_progress == null)
            {
                _progress = new BattlePassProgress(season.Id);
                _repository.SaveProgress(_progress);
            }

            UpdateLevel();

            _events.Publish(BattlePassEvent.SeasonStarted());
        }

        // =========================================================
        // BASIC INFO
        // =========================================================

        public BattlePassProgress GetProgress()
        {
            return _progress;
        }

        public int GetCurrentLevel()
        {
            return _progress.CurrentLevel;
        }

        public int GetXP()
        {
            return _progress.XP;
        }

        public bool HasPremium()
        {
            return _progress.HasPremium;
        }

        // =========================================================
        // XP
        // =========================================================

        public void AddXP(int amount)
        {
            if (amount <= 0)
                return;

            int oldLevel = _progress.CurrentLevel;

            _progress.XP += amount;

            UpdateLevel();

            _repository.SaveProgress(_progress);

            _events.Publish(BattlePassEvent.XPAdded(amount));

            if (_progress.CurrentLevel > oldLevel)
            {
                _events.Publish(BattlePassEvent.LevelUp(_progress.CurrentLevel));
            }
        }

        private void UpdateLevel()
        {
            int level = BattlePassCalculator.CalculateLevel(
                _progress.XP,
                _season.Levels);

            _progress.CurrentLevel = level;
        }

        // =========================================================
        // LEVEL PROGRESS
        // =========================================================

        public int GetXPForNextLevel()
        {
            var next = _season.Levels
                .FirstOrDefault(x => x.Level == _progress.CurrentLevel + 1);

            if (next == null)
                return 0;

            return next.RequiredXP;
        }

        public float GetLevelProgress()
        {
            int currentXP = _progress.XP;

            var currentLevelData = _season.Levels
                .FirstOrDefault(x => x.Level == _progress.CurrentLevel);

            var nextLevelData = _season.Levels
                .FirstOrDefault(x => x.Level == _progress.CurrentLevel + 1);

            if (nextLevelData == null)
                return 1f;

            int startXP = currentLevelData.RequiredXP;
            int endXP = nextLevelData.RequiredXP;

            return (float)(currentXP - startXP) / (endXP - startXP);
        }

        // =========================================================
        // REWARD CLAIM
        // =========================================================

        public bool CanClaimFreeReward(int level)
        {
            if (level > _progress.CurrentLevel)
                return false;

            if (_progress.IsFreeRewardClaimed(level))
                return false;

            return true;
        }

        public BattlePassReward ClaimFreeReward(int level)
        {
            if (!CanClaimFreeReward(level))
                return null;

            var levelData = _season.Levels.First(x => x.Level == level);

            _progress.ClaimFreeReward(level);

            _repository.SaveProgress(_progress);

            _events.Publish(BattlePassEvent.FreeRewardClaimed(level));

            return levelData.FreeReward;
        }

        public bool CanClaimPremiumReward(int level)
        {
            if (!_progress.HasPremium)
                return false;

            if (level > _progress.CurrentLevel)
                return false;

            if (_progress.IsPremiumRewardClaimed(level))
                return false;

            return true;
        }

        public BattlePassReward ClaimPremiumReward(int level)
        {
            if (!CanClaimPremiumReward(level))
                return null;

            var levelData = _season.Levels.First(x => x.Level == level);

            _progress.ClaimPremiumReward(level);

            _repository.SaveProgress(_progress);

            _events.Publish(BattlePassEvent.PremiumRewardClaimed(level));

            return levelData.PremiumReward;
        }

        // =========================================================
        // CLAIM UTILITIES
        // =========================================================

        public void ClaimAllRewards()
        {
            foreach (var level in _season.Levels)
            {
                if (CanClaimFreeReward(level.Level))
                    ClaimFreeReward(level.Level);

                if (CanClaimPremiumReward(level.Level))
                    ClaimPremiumReward(level.Level);
            }

            _events.Publish(BattlePassEvent.RewardClaimedAll());
        }

        public bool HasClaimableRewards()
        {
            foreach (var level in _season.Levels)
            {
                if (CanClaimFreeReward(level.Level))
                    return true;

                if (CanClaimPremiumReward(level.Level))
                    return true;
            }

            return false;
        }

        // =========================================================
        // LEVEL INFO (NEW)
        // =========================================================

        public BattlePassLevel GetLevel(int level)
        {
            return _season.Levels.FirstOrDefault(x => x.Level == level);
        }

        public bool IsLevelUnlocked(int level)
        {
            return level <= _progress.CurrentLevel;
        }

        // =========================================================
        // PREMIUM
        // =========================================================

        public void UnlockPremium()
        {
            if (_progress.HasPremium)
                return;

            _progress.HasPremium = true;

            _repository.SaveProgress(_progress);

            _events.Publish(BattlePassEvent.PremiumUnlocked());
        }

        // =========================================================
        // SEASON
        // =========================================================

        public bool IsSeasonActive(DateTime now)
        {
            return _season.IsActive(now);
        }

        public TimeSpan GetRemainingTime(DateTime now)
        {
            return _season.EndTime - now;
        }

        // NEW

        public bool IsSeasonEnded(DateTime now)
        {
            return now > _season.EndTime;
        }

        // =========================================================
        // DISPOSE
        // =========================================================

        public void Dispose()
        {
        }
    }
}