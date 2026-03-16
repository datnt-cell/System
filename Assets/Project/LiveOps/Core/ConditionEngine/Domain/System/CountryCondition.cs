namespace ConditionEngine.Domain
{
    /// <summary>
    /// Condition kiểm tra country của player
    /// Dùng cho geo targeting
    /// </summary>
    public class CountryCondition : ConditionBase
    {
        public string CountryCode;

        public CountryCondition(string countryCode)
        {
            CountryCode = countryCode;
        }

        public override bool Evaluate(IConditionContext context)
        {
            return context.Country == CountryCode;
        }
    }
}