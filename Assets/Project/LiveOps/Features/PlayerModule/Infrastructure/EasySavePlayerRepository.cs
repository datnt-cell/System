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
            return new PlayerData
            {
                Level = 1,
                Stage = 1,
                SessionCount = 0,
                IsNewUser = true,
                Country = "US",
                Segment = PlayerSegment.NonSpender,
                DaysSinceInstall = 0,
                TotalPlayTimeMinutes = 0
            };
        }
    }
}