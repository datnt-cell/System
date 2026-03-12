using System;
using System.Collections.Generic;

/// <summary>
/// Service quản lý Offer Groups
/// </summary>
public class GameOfferGroupService
{
    private GameOfferGroupGlobalConfig config;

    private Dictionary<string, GameOfferGroupRuntimeData> runtime
        = new();

    public GameOfferGroupService(GameOfferGroupGlobalConfig config)
    {
        this.config = config;
    }

    /// <summary>
    /// Activate group
    /// </summary>
    public void ActivateGroup(string groupId)
    {
        var group = config.Get(groupId);

        if (group == null)
            return;

        if (runtime.ContainsKey(groupId))
            return;

        runtime[groupId] = new GameOfferGroupRuntimeData
        {
            GroupId = groupId,
            StartTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            IsActivated = !group.WaitForActivation
        };
    }

    /// <summary>
    /// Lấy offer có thể mua trong group
    /// </summary>
    public string GetAvailableOffer(string groupId)
    {
        if (!runtime.TryGetValue(groupId, out var data))
            return null;

        var group = config.Get(groupId);

        if (group == null)
            return null;

        switch (group.Type)
        {
            case OfferGroupType.ChainDeals:
                if (data.CurrentIndex >= group.OfferIds.Count)
                    return null;

                return group.OfferIds[data.CurrentIndex];

            default:
                return null;
        }
    }

    /// <summary>
    /// Player mua offer trong group
    /// </summary>
    public void Purchase(string groupId, string offerId)
    {
        if (!runtime.TryGetValue(groupId, out var data))
            return;

        var group = config.Get(groupId);

        if (group == null)
            return;

        switch (group.Type)
        {
            case OfferGroupType.ChainDeals:

                if (group.OfferIds[data.CurrentIndex] == offerId)
                {
                    data.CurrentIndex++;
                }

                break;

            case OfferGroupType.OnlyOnePurchase:

                data.PurchasedOffers.Add(offerId);

                break;

            case OfferGroupType.PurchaseEachOfferOnce:

                data.PurchasedOffers.Add(offerId);

                break;
        }
    }
}