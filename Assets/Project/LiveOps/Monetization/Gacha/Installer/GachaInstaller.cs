using System.Linq;
using GameSystems.Random.Providers;
using GachaSystem.Application.Services;
using GachaSystem.Domain.Models;
using GachaSystem.Infrastructure.Events;

namespace GachaSystem.Installer
{
    public class GachaInstaller
    {
        public GachaInstallResult Install()
        {
            // =========================
            // RANDOM
            // =========================

            var random = new UnityRandomProvider();

            // =========================
            // EVENTS
            // =========================

            var events = new GachaEvents();

            // =========================
            // SERVICE
            // =========================

            var service = new GachaService(
                random,
                events
            );

            // =========================
            // LOAD CONFIG
            // =========================

            var config = GachaGlobalConfig.Instance;

            foreach (var poolConfig in config.Pools)
            {
                var pool = ConvertPool(poolConfig);
                service.RegisterPool(pool);
            }

            // =========================
            // RESULT
            // =========================

            return new GachaInstallResult
            {
                Service = service,
                Events = events
            };
        }

        // =========================
        // CONVERT
        // =========================

        private GachaPool ConvertPool(GachaPoolConfigData poolConfig)
        {
            var items = poolConfig.Items
                .Select(ConvertItem)
                .ToList();

            return new GachaPool(
                poolConfig.Id,
                items
            );
        }

        private GachaItem ConvertItem(GachaItemConfigData config)
        {
            string rewardId = config.RewardType switch
            {
                GachaRewardType.Currency => config.CurrencyId,
                GachaRewardType.CurrencyBundle => config.BundleId,
                _ => null
            };

            int amount = config.RewardType == GachaRewardType.Currency
                ? config.Amount
                : 1;

            return new GachaItem(
                config.RewardType,
                rewardId,
                amount,
                config.Weight
            );
        }
    }
}