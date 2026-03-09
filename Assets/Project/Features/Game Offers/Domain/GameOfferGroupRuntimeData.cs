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

    public bool IsExpired(int duration)
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        return now > StartTime + duration;
    }
}