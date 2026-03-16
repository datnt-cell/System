using System;

public enum AdsEventType
{
    InterstitialShown,
    RewardedStart,
    RewardedResult,
    RewardedError,
    RemoveAds
}

public class AdsEvent
{
    public AdsEventType Type;

    public bool Success;
    public double Revenue;

    public Exception Exception;

    public static AdsEvent Interstitial(double revenue)
    {
        return new AdsEvent
        {
            Type = AdsEventType.InterstitialShown,
            Revenue = revenue
        };
    }

    public static AdsEvent RewardStart()
    {
        return new AdsEvent
        {
            Type = AdsEventType.RewardedStart
        };
    }

    public static AdsEvent RewardResult(bool success, double revenue)
    {
        return new AdsEvent
        {
            Type = AdsEventType.RewardedResult,
            Success = success,
            Revenue = revenue
        };
    }

    public static AdsEvent RewardError(Exception e)
    {
        return new AdsEvent
        {
            Type = AdsEventType.RewardedError,
            Exception = e
        };
    }

    public static AdsEvent RemoveAds()
    {
        return new AdsEvent
        {
            Type = AdsEventType.RemoveAds
        };
    }
}