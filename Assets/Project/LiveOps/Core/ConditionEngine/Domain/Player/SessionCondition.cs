namespace ConditionEngine.Domain
{
    /// <summary>
    /// Condition kiểm tra số session player đã chơi
    /// Ví dụ:
    /// chỉ hiện tutorial trong 3 session đầu
    /// </summary>
    public class SessionCondition : ConditionBase
    {
        public int MinSession;

        public int MaxSession;

        public SessionCondition(int minSession, int maxSession = int.MaxValue)
        {
            MinSession = minSession;
            MaxSession = maxSession;
        }

        public override bool Evaluate(IConditionContext context)
        {
            int session = context.SessionCount;

            return session >= MinSession && session <= MaxSession;
        }
    }
}