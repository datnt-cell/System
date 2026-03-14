namespace ConditionEngine.Infrastructure
{
    /// <summary>
    /// Provider cung cấp thông tin player
    /// Thực tế sẽ lấy dữ liệu từ PlayerData / SaveData
    /// </summary>
    public class PlayerProvider
    {
        // =====================
        // PLAYER PROGRESS
        // =====================

        /// <summary>
        /// Level hiện tại của player
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Stage / Level map hiện tại
        /// </summary>
        public int Stage { get; set; }

        /// <summary>
        /// Số session player đã chơi
        /// </summary>
        public int SessionCount { get; set; }

        /// <summary>
        /// Player có phải new user không
        /// </summary>
        public bool IsNewUser { get; set; }

        // =====================
        // PLAYER INFO
        // =====================

        /// <summary>
        /// Country của player (ISO code)
        /// </summary>
        public string Country { get; set; }


        /// <summary>
        /// Segment của player
        /// Ví dụ: whale / spender / non_spender
        /// </summary>
        public string Segment { get; set; }

        // =====================
        // TIME
        // =====================

        /// <summary>
        /// Số ngày kể từ khi player cài game
        /// </summary>
        public int DaysSinceInstall { get; set; }

        /// <summary>
        /// Tổng play time của player (phút)
        /// </summary>
        public int TotalPlayTimeMinutes { get; set; }
    }
}