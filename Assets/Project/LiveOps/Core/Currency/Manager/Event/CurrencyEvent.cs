using CurrencySystem.Domain;

namespace CurrencySystem.Application
{
    public enum CurrencyEventType
    {
        Added,
        Spent
    }

    /// <summary>
    /// Event cho mỗi thao tác currency, chứa full thông tin như CurrencyResponse.
    /// </summary>
    public class CurrencyEvent
    {
        public CurrencyEventType Type { get; }
        public CurrencyId CurrencyId { get; }
        public int Amount { get; }
        public int Balance { get; }
        public int ChangedAmount { get; }
        public string Source { get; }
        public bool Success { get; }
        public Errors? ErrorCode { get; }
        public string ErrorMessage { get; }

        private CurrencyEvent(
            CurrencyEventType type,
            CurrencyId currencyId,
            int amount,
            int balance,
            int changedAmount,
            string source,
            bool success,
            Errors? errorCode = null,
            string errorMessage = null)
        {
            Type = type;
            CurrencyId = currencyId;
            Amount = amount;
            Balance = balance;
            ChangedAmount = changedAmount;
            Source = source;
            Success = success;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Factory tạo event Add thành công.
        /// </summary>
        public static CurrencyEvent Add(
            CurrencyId id,
            int amount,
            int balance,
            int changedAmount,
            string source)
        {
            return new CurrencyEvent(
                CurrencyEventType.Added,
                id,
                amount,
                balance,
                changedAmount,
                source,
                true
            );
        }

        /// <summary>
        /// Factory tạo event Spend thành công.
        /// </summary>
        public static CurrencyEvent Spend(
            CurrencyId id,
            int amount,
            int balance,
            int changedAmount,
            string source)
        {
            return new CurrencyEvent(
                CurrencyEventType.Spent,
                id,
                amount,
                balance,
                changedAmount,
                source,
                true
            );
        }

        /// <summary>
        /// Factory tạo event lỗi.
        /// </summary>
        public static CurrencyEvent Fail(
            CurrencyId id,
            int amount,
            string source,
            Errors errorCode,
            string errorMessage)
        {
            return new CurrencyEvent(
                amount >= 0 ? CurrencyEventType.Added : CurrencyEventType.Spent,
                id,
                amount,
                balance: 0,
                changedAmount: 0,
                source,
                success: false,
                errorCode: errorCode,
                errorMessage: errorMessage
            );
        }

        public override string ToString()
        {
            if (Success)
                return $"[{Type}] Id: {CurrencyId}, Amount: {Amount}, Changed: {ChangedAmount}, Balance: {Balance}, Source: {Source}";
            else
                return $"[Failed {Type}] Id: {CurrencyId}, Amount: {Amount}, Source: {Source}, Error: {ErrorCode} - {ErrorMessage}";
        }
    }
}