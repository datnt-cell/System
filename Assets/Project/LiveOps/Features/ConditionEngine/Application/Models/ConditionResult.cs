namespace ConditionEngine.Application
{
    /// <summary>
    /// Kết quả evaluate condition
    /// Có thể dùng để debug hoặc log
    /// </summary>
    public class ConditionResult
    {
        /// <summary>
        /// Condition pass hay fail
        /// </summary>
        public bool Passed;

        /// <summary>
        /// Message debug (optional)
        /// </summary>
        public string Message;

        public ConditionResult(bool passed, string message = "")
        {
            Passed = passed;
            Message = message;
        }
    }
}