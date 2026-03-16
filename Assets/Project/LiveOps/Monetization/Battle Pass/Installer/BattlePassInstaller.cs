using BattlePassModule.Application;
using BattlePassModule.Domain;
using BattlePassModule.Infrastructure.Config;
using BattlePassModule.Infrastructure.Repository;
using System.Linq;

namespace BattlePassModule
{
    public class BattlePassInstaller
    {
        public BattlePassInstallResult Install()
        {
            var repository = new EasySaveBattlePassRepository();

            var events = new BattlePassEvents();

            var service = new BattlePassService(
                repository,
                events
            );

            // ===== LOAD CONFIG =====

            var config = BattlePassGlobalConfig.Instance;

            var seasonConfig = config.Seasons.First();

            var season = ConvertSeason(seasonConfig);

            service.Initialize(season);

            return new BattlePassInstallResult(
                service,
                events
            );
        }

        // =========================
        // CONVERT SEASON
        // =========================

        private BattlePassSeason ConvertSeason(
            BattlePassSeasonConfig config)
        {
            var levels = config.Levels
                .Select(x => new BattlePassLevel(
                    x.Level,
                    x.RequiredXP,
                    ConvertReward(x.FreeReward),
                    ConvertReward(x.PremiumReward)
                ))
                .ToList();

            return new BattlePassSeason(
                config.Id,
                config.StartTime,
                config.EndTime,
                levels
            );
        }

        // =========================
        // CONVERT REWARD
        // =========================

        private BattlePassReward ConvertReward(
            BattlePassRewardConfig config)
        {
            if (config == null)
                return null;

            switch (config.RewardType)
            {
                case BattlePassRewardType.Currency:
                    return BattlePassReward.Currency(
                        config.CurrencyId,
                        config.Amount
                    );

                case BattlePassRewardType.CurrencyBundle:
                    return BattlePassReward.Bundle(
                        config.BundleId
                    );
            }

            return null;
        }
    }
}