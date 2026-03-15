using System;

namespace PlayerSystem.Domain
{
    /// <summary>
    /// Entity chứa toàn bộ trạng thái của player
    /// Đây là dữ liệu cốt lõi của game
    /// Không phụ thuộc Unity hay SDK
    /// </summary>
    [Serializable]
    public class PlayerData
    {
        // PROGRESS
        public int Level;
        public int Stage;
        public int TutorialStep;

        // SESSION
        public int SessionCount;
        public int TotalPlayTimeSeconds;

        // USER
        public bool IsNewUser;
        public bool DontDisturb;

        // REGION
        public string Country;
        public string SystemLanguage;

        // TRAFFIC
        public string TrafficSource;
        public string TrafficCampaign;

        // IDENTITY
        public string ProfileId;
        public string DeviceId;
        public string CustomId;

        // DEVICE
        public string Platform;
        public int AppVersion;
        public string EngineVersion;

        // TIME
        public long FirstLoginTime;
        public long LastLoginTime;
        public long CurrentDay;
        public long LastPurchaseTime;
    }
}