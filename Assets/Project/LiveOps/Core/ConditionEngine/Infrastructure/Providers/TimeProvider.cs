using System;

namespace ConditionEngine.Infrastructure
{
    /// <summary>
    /// Provider cung cấp thời gian hiện tại
    /// Có thể thay bằng NetworkTime sau này
    /// </summary>
    public class TimeProvider
    {
        public DateTime Now => DateTime.UtcNow;
    }
}