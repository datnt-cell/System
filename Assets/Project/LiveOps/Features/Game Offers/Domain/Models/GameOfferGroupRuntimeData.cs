using System;
using System.Collections.Generic;

/// <summary>
/// Runtime data của Offer Group
/// </summary>
[Serializable]
public class GameOfferGroupRuntimeData
{
    public string GroupId;

    public long StartTime;

    public bool IsActivated;

    /// <summary>
    /// index của chain deal
    /// </summary>
    public int CurrentIndex;

    /// <summary>
    /// các offer đã mua
    /// </summary>
    public HashSet<string> PurchasedOffers = new();

    public bool IsExpired(TimeSpan duration)
    {
        // duration = 0 => infinity (never expire)
        if (duration == TimeSpan.Zero)
            return false;

        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long endTime = StartTime + (long)duration.TotalSeconds;

        return now > endTime;
    }
}