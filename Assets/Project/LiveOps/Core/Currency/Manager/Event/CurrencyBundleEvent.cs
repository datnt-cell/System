using R3;
using CurrencySystem.Domain;

namespace CurrencySystem.Application
{
    // Kiểu event riêng cho bundle
    public enum CurrencyBundleEventType
    {
        Opened
    }

    /// <summary>
    /// Event chi tiết cho mỗi reward khi mở bundle
    /// </summary>
    public class CurrencyBundleEvent
    {
        public CurrencyBundleEventType Type { get; }
        public string BundleId { get; }
        public CurrencyId RewardId { get; }
        public int Amount { get; }
        public int Balance { get; }
        public int ChangedAmount { get; }
        public string Source { get; }
        public bool Success { get; }
        public Errors? ErrorCode { get; }
        public string ErrorMessage { get; }

        private CurrencyBundleEvent(
            CurrencyBundleEventType type,
            string bundleId,
            CurrencyId rewardId,
            int amount,
            int balance,
            int changedAmount,
            string source,
            bool success,
            Errors? errorCode = null,
            string errorMessage = null)
        {
            Type = type;
            BundleId = bundleId;
            RewardId = rewardId;
            Amount = amount;
            Balance = balance;
            ChangedAmount = changedAmount;
            Source = source;
            Success = success;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Factory event mở bundle thành công
        /// </summary>
        public static CurrencyBundleEvent Opened(
            string bundleId,
            CurrencyId rewardId,
            int amount,
            int balance,
            int changedAmount,
            string source)
        {
            return new CurrencyBundleEvent(
                CurrencyBundleEventType.Opened,
                bundleId,
                rewardId,
                amount,
                balance,
                changedAmount,
                source,
                success: true
            );
        }

        /// <summary>
        /// Factory event mở bundle thất bại
        /// </summary>
        public static CurrencyBundleEvent Fail(
            string bundleId,
            CurrencyId rewardId,
            int amount,
            string source,
            Errors errorCode,
            string errorMessage)
        {
            return new CurrencyBundleEvent(
                CurrencyBundleEventType.Opened,
                bundleId,
                rewardId,
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
                return $"[Opened] Bundle: {BundleId}, Reward: {RewardId}, Amount: {Amount}, Changed: {ChangedAmount}, Balance: {Balance}, Source: {Source}";
            else
                return $"[Failed Open] Bundle: {BundleId}, Reward: {RewardId}, Amount: {Amount}, Source: {Source}, Error: {ErrorCode} - {ErrorMessage}";
        }
    }

    public interface ICurrencyBundleEvents
    {
        Observable<CurrencyBundleEvent> Stream { get; }
    }

    public class CurrencyBundleEvents : ICurrencyBundleEvents
    {
        private readonly Subject<CurrencyBundleEvent> _events = new();

        public Observable<CurrencyBundleEvent> Stream => _events.AsObservable();

        public void Publish(CurrencyBundleEvent evt)
        {
            _events.OnNext(evt);
        }
    }
}