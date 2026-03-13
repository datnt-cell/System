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
    public OfferPurchaseError CanPurchase(string groupId, string offerId)
    {
        if (string.IsNullOrEmpty(groupId) || string.IsNullOrEmpty(offerId))
            return OfferPurchaseError.OfferNotFound;

        var availableOffer = GetAvailableOffer(groupId);

        if (availableOffer == null)
            return OfferPurchaseError.OfferNotActive;

        if (availableOffer != offerId)
            return OfferPurchaseError.PurchaseNotAllowed;

        return OfferPurchaseError.None;
    }

    /// <summary>
    /// Player mua offer
    /// </summary>
    public OfferPurchaseError Purchase(string groupId, string offerId)
    {
        var data = state.Get(groupId);
        if (data == null)
        {
            events?.OnGroupPurchaseFailed(groupId, offerId, "Group not found");
            return OfferPurchaseError.OfferNotFound;
        }

        var group = config.Get(groupId);
        if (group == null)
        {
            events?.OnGroupPurchaseFailed(groupId, offerId, "Config not found");
            return OfferPurchaseError.OfferNotFound;
        }

        var check = CanPurchase(groupId, offerId);
        if (check != OfferPurchaseError.None)
        {
            events?.OnGroupPurchaseFailed(groupId, offerId, check.ToString());
            return check;
        }

        ApplyPurchase(group, data, offerId);

        Save();

        events?.OnGroupOfferPurchased(groupId, offerId);

        return OfferPurchaseError.None;
    }

    private void ApplyPurchase(
    GameOfferGroupConfigData group,
    GameOfferGroupRuntimeData data,
    string offerId)
    {
        switch (group.Type)
        {
            case OfferGroupType.ChainDeals:
                data.CurrentIndex++;
                break;

            case OfferGroupType.OnlyOnePurchase:
            case OfferGroupType.PurchaseEachOfferOnce:
                data.PurchasedOffers.Add(offerId);
                break;
        }
    }
}