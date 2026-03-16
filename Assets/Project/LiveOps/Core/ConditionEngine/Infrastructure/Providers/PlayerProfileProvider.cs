using PlayerSystem.Application;
using ConditionEngine.Domain;
using System;

namespace ConditionEngine.Infrastructure
{
    /// <summary>
    /// Adapter lấy dữ liệu từ PlayerService
    /// </summary>
    public class PlayerProvider : IPlayerProvider
    {
        private readonly PlayerService _playerService;

        public PlayerProvider(PlayerService playerService)
        {
            _playerService = playerService;
        }

        // =====================
        // PROGRESS
        // =====================

        public int Level => _playerService.GetLevel();

        public int Stage => _playerService.GetStage();

        public int TutorialStep => _playerService.GetTutorialStep();

        // =====================
        // SESSION
        // =====================

        public int SessionCount => _playerService.GetSessionCount();

        public int TotalPlayTimeSeconds => _playerService.GetPlayTime();

        // =====================
        // USER
        // =====================

        public bool IsNewUser => _playerService.IsNewUser();

        public bool DontDisturb => _playerService.DontDisturb();

        // =====================
        // REGION
        // =====================

        public string Country => _playerService.GetCountry();

        public int DaysSinceInstall => (int)_playerService.GetCurrentDay();

        // =====================
        // DEVICE
        // =====================

        public string Platform => _playerService.GetPlatform();
        public int AppVersion => _playerService.GetAppVersion();
        public string EngineVersion => _playerService.GetEngineVersion();

        // =====================
        // TRAFFIC
        // =====================

        public string TrafficSource => _playerService.GetTrafficSource();

        public string TrafficCampaign => _playerService.GetTrafficCampaign();

        // =====================
        // PURCHASE
        // =====================

        public long TimeSincePurchase
        {
            get
            {
                long lastPurchase = _playerService.GetLastPurchaseTime();

                if (lastPurchase <= 0)
                    return -1;

                long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                return now - lastPurchase;
            }
        }
    }
}