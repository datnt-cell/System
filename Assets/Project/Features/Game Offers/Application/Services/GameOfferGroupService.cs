using System;
using System.Collections.Generic;
using GameOfferSystem.Infrastructure;
using GameOfferSystem.Domain;

/// <summary>
/// Service quản lý toàn bộ business logic của Offer Groups
/// </summary>
public class GameOfferGroupService
{
    private readonly GameOfferGroupState state;
    private readonly IGameOfferGroupRepository repository;
    private readonly GameOfferGroupGlobalConfig config;
    private readonly IGameOfferGroupEvents events;

    public GameOfferGroupService(
        GameOfferGroupState state,
        IGameOfferGroupRepository repository,
        GameOfferGroupGlobalConfig config,
        IGameOfferGroupEvents events)
    {
        this.state = state;
        this.repository = repository;
        this.config = config;
        this.events = events;

        Load();
    }

    /// <summary>
    /// Load runtime data
    /// </summary>
    private void Load()
    {
        var data = repository.Load();

        foreach (var group in data)
        {
            state.Add(group);
        }
    }

    /// <summary>
    /// Save runtime
    /// </summary>
    private void Save()
    {
        repository.Save(new List<GameOfferGroupRuntimeData>(state.Values));
    }

    /// <summary>
    /// Activate group
    /// </summary>
    public GameOfferGroupRuntimeData ActivateGroup(string groupId)
    {
        var group = config.Get(groupId);

        if (group == null)
            return null;

        if (state.ContainsKey(groupId))
            return state.Get(groupId);

        var runtime = new GameOfferGroupRuntimeData
        {
            GroupId = groupId,
            StartTime = group.WaitForActivation
                ? 0
                : DateTimeOffset.UtcNow.ToUnixTimeSeconds(),

            IsActivated = !group.WaitForActivation
        };

        state.Add(runtime);

        Save();

        events?.OnGroupActivated(runtime);

        return runtime;
    }

    /// <summary>
    /// Player nhìn thấy group → bắt đầu timer
    /// </summary>
    public void MarkSeen(string groupId)
    {
        var data = state.Get(groupId);

        if (data == null)
            return;

        if (!data.IsActivated)
        {
            data.IsActivated = true;
            data.StartTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            Save();
        }
    }

    /// <summary>
    /// Lấy offer hiện tại có thể mua
    /// </summary>
    public string GetAvailableOffer(string groupId)
    {
        var data = state.Get(groupId);

        if (data == null)
            return null;

        if (!data.IsActivated)
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

            case OfferGroupType.OnlyOnePurchase:

                foreach (var offer in group.OfferIds)
                {
                    if (!data.PurchasedOffers.Contains(offer))
                        return offer;
                }

                return null;

            case OfferGroupType.PurchaseEachOfferOnce:

                foreach (var offer in group.OfferIds)
                {
                    if (!data.PurchasedOffers.Contains(offer))
                        return offer;
                }

                return null;
        }

        return null;
    }

    /// <summary>
    /// Kiểm tra có thể mua offer trong group
    /// </summary>
    public bool CanPurchase(string groupId, string offerId)
    {
        var offer = GetAvailableOffer(groupId);

        if (offer == null)
            return false;

        return offer == offerId;
    }

    /// <summary>
    /// Player mua offer
    /// </summary>
    public bool Purchase(string groupId, string offerId)
    {
        var data = state.Get(groupId);

        if (data == null)
        {
            events?.OnGroupPurchaseFailed(groupId, offerId, "Group not found");
            return false;
        }

        var group = config.Get(groupId);

        if (group == null)
        {
            events?.OnGroupPurchaseFailed(groupId, offerId, "Config not found");
            return false;
        }

        if (!CanPurchase(groupId, offerId))
        {
            events?.OnGroupPurchaseFailed(groupId, offerId, "Purchase not allowed");
            return false;
        }

        switch (group.Type)
        {
            case OfferGroupType.ChainDeals:

                data.CurrentIndex++;

                break;

            case OfferGroupType.OnlyOnePurchase:

                data.PurchasedOffers.Add(offerId);

                break;

            case OfferGroupType.PurchaseEachOfferOnce:

                data.PurchasedOffers.Add(offerId);

                break;
        }

        Save();

        events?.OnGroupOfferPurchased(groupId, offerId);

        return true;
    }
}