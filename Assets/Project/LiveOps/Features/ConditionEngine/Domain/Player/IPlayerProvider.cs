namespace ConditionEngine.Domain
{
    /// <summary>
    /// Interface cung cấp thông tin Player
    /// ConditionEngine chỉ phụ thuộc interface này
    /// </summary>
    public interface IPlayerProvider
    {
        int Level { get; }

        int Stage { get; }

        int SessionCount { get; }

        bool IsNewUser { get; }

        string Country { get; }

        string Segment { get; }

        int DaysSinceInstall { get; }

        int TotalPlayTimeMinutes { get; }
    }
}