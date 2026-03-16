namespace ConditionEngine.Domain
{
    /// <summary>
    /// Interface cung cấp thông tin Player
    /// ConditionEngine chỉ phụ thuộc interface này
    /// </summary>
    public interface IPlayerProvider
    {
        // =====================
        // PROGRESS
        // =====================

        int Level { get; }

        int Stage { get; }

        int TutorialStep { get; }

        // =====================
        // SESSION
        // =====================

        int SessionCount { get; }

        int TotalPlayTimeSeconds { get; }

        // =====================
        // USER
        // =====================

        bool IsNewUser { get; }

        bool DontDisturb { get; }

        // =====================
        // REGION
        // =====================

        string Country { get; }

        int DaysSinceInstall { get; }

        // =====================
        // TRAFFIC
        // =====================

        string TrafficSource { get; }

        string TrafficCampaign { get; }

        // =====================
        // PURCHASE
        // =====================

        long TimeSincePurchase { get; }
    }
}