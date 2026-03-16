namespace GameEventModule.Domain
{
    /// <summary>
    /// Cách event kết thúc
    /// </summary>
    public enum EventFinishType
    {
        /// <summary>
        /// Event kết thúc khi condition false
        /// </summary>
        Condition,

        /// <summary>
        /// Event kết thúc sau một khoảng duration
        /// </summary>
        Duration
    }
}