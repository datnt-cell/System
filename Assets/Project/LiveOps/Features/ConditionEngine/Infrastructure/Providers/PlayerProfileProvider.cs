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

        public int Level => _playerService.Level.Value;

        public int Stage => _playerService.Stage.Value;

        public int TutorialStep => _playerService.TutorialStep.Value;

        // =====================
        // SESSION
        // =====================

        public int SessionCount => _playerService.SessionCount.Value;

        public int TotalPlayTimeSeconds => _playerService.PlayTimeSeconds.Value;

        // =====================
        // USER
        // =====================

        public bool IsNewUser => _playerService.IsNewUser.Value;

        public bool DontDisturb => _playerService.DontDisturb.Value;

        // =====================
        // REGION
        // =====================

        public string Country => _playerService.Country.Value;

        public string Segment => "";

        public int DaysSinceInstall => (int)_playerService.CurrentDay.Value;

        // =====================
        // DEVICE
        // =====================

        public string Platform => _playerService.Platform.Value;
        public int AppVersion => _playerService.AppVersion.Value;
        public string EngineVersion => _playerService.EngineVersion.Value;

        // =====================
        // TRAFFIC
        // =====================

        public string TrafficSource => _playerService.TrafficSource.Value;

        public string TrafficCampaign => _playerService.TrafficCampaign.Value;

        // =====================
        // PURCHASE
        // =====================

        public long TimeSincePurchase
        {
            get
            {
                long lastPurchase = _playerService.LastPurchaseTime.Value;

                if (lastPurchase <= 0)
                    return -1;

                long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                return now - lastPurchase;
            }
        }
    }
}