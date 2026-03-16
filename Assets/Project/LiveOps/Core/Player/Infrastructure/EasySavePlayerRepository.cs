using System;
using PlayerSystem.Domain;

namespace PlayerSystem.Infrastructure
{
    /// <summary>
    /// Repository lưu PlayerData bằng EasySave vào file riêng
    /// </summary>
    public class EasySavePlayerRepository : IPlayerRepository
    {
        private const string FILE = "player_data.es3";
        private const string KEY = "player_data";

        public PlayerData Load()
        {
            if (ES3.KeyExists(KEY, FILE))
                return ES3.Load<PlayerData>(KEY, FILE);

            return CreateDefault();
        }

        public void Save(PlayerData data)
        {
            ES3.Save(KEY, data, FILE);
        }

        /// <summary>
        /// Tạo PlayerData mặc định khi chưa có save
        /// </summary>
        private PlayerData CreateDefault()
        {
            long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            return new PlayerData
            {
                // =====================
                // PROGRESS
                // =====================

                Level = 1,
                Stage = 1,
                TutorialStep = 0,

                // =====================
                // SESSION
                // =====================

                SessionCount = 0,
                TotalPlayTimeSeconds = 0,

                // =====================
                // USER
                // =====================

                IsNewUser = true,
                DontDisturb = false,

                // =====================
                // REGION
                // =====================

                Country = "US",
                SystemLanguage = UnityEngine.Application.systemLanguage.ToString(),

                // =====================
                // TRAFFIC
                // =====================

                TrafficSource = "Organic",
                TrafficCampaign = "",

                // =====================
                // IDENTITY
                // =====================

                ProfileId = Guid.NewGuid().ToString(),
                DeviceId = UnityEngine.SystemInfo.deviceUniqueIdentifier,
                CustomId = "",

                // =====================
                // DEVICE
                // =====================

                Platform = UnityEngine.Application.platform.ToString(),
                AppVersion = HelperLiveOps.GetBuildNumber(),
                EngineVersion = UnityEngine.Application.unityVersion,

                // =====================
                // TIME
                // =====================

                FirstLoginTime = now,
                LastLoginTime = now,
                CurrentDay = now / 86400,
                LastPurchaseTime = -1
            };
        }
    }
}