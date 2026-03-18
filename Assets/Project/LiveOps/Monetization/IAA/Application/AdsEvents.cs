using R3;
using System;

namespace AdsSystem.Application
{
    // =========================
    // Update AdsEvent không revenue
    // =========================
    public enum AdsEventType
    {
        Interstitial,
        Rewarded,
        RemoveAds,
        Fail
    }

    public class AdsEvent
    {
        public AdsEventType Type { get; }
        public AdType AdType { get; }
        public bool Success { get; }
        public string ErrorMessage { get; }

        private AdsEvent(AdsEventType type, AdType adType, bool success, string errorMessage = null)
        {
            Type = type;
            AdType = adType;
            Success = success;
            ErrorMessage = errorMessage;
        }

        public static AdsEvent Interstitial() => new AdsEvent(AdsEventType.Interstitial, AdType.Interstitial, true);
        public static AdsEvent Rewarded(bool success) => new AdsEvent(AdsEventType.Rewarded, AdType.Rewarded, success);
        public static AdsEvent RemoveAds() => new AdsEvent(AdsEventType.RemoveAds, AdType.Custom, true);
        public static AdsEvent Fail(AdType adType, string error) => new AdsEvent(AdsEventType.Fail, adType, false, error);
    }

    public interface IAdsEvents
    {
        Observable<AdsEvent> Stream { get; }
    }

    public class AdsEvents : IAdsEvents
    {
        private readonly Subject<AdsEvent> _events = new();
        public Observable<AdsEvent> Stream => _events.AsObservable();
        public void Publish(AdsEvent evt) => _events.OnNext(evt);
    }
}