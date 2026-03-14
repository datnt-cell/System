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
        // =====================
        // PLAYER PROGRESS
        // =====================

        /// <summary>
        /// Level hiện tại của player
        /// </summary>
        public int Level;

        /// <summary>
        /// Stage / Map hiện tại player đang chơi
        /// </summary>
        public int Stage;

        /// <summary>
        /// Tổng số session player đã chơi
        /// </summary>
        public int SessionCount;

        /// <summary>
        /// Player có phải new user hay không
        /// </summary>
        public bool IsNewUser;

        // =====================
        // PLAYER INFO
        // =====================

        /// <summary>
        /// Country của player (ISO code)
        /// Ví dụ: VN, US, JP
        /// </summary>
        public string Country;

        /// <summary>
        /// Segment của player
        /// Ví dụ: whale / spender / non_spender
        /// </summary>
        public string Segment;

        // =====================
        // TIME
        // =====================

        /// <summary>
        /// Số ngày kể từ khi player cài game
        /// </summary>
        public int DaysSinceInstall;

        /// <summary>
        /// Tổng playtime của player (tính bằng phút)
        /// </summary>
        public int TotalPlayTimeMinutes;

        public long FirstInstallTimestamp;
    }
}