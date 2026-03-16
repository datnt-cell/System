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

    private bool IsInfinite(GameOfferGroupConfigData group)
    {
        return group.Duration.TimeSpan <= TimeSpan.Zero;
    }

    private void Load()
    {
        var data = repository.Load();

        if (data == null)
            return;

        foreach (var group in data)
        {
            state.Add(group);
        }
    }

    private void Save()
    {
        repository.Save(new List<GameOfferGroupRuntimeData>(state.Values));
    }

    /// Activate group
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

    /// Player nhìn thấy group → bắt đầu timer
    public void MarkSeen(string groupId)
    {
        var data = state.Get(groupId);

        if (data == null || data.IsActivated)
            return;

        data.IsActivated = true;
        data.StartTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        Save();
    }

    private bool IsExpired(GameOfferGroupRuntimeData data, GameOfferGroupConfigData group)
    {
        if (!data.IsActivated)
            return false;

        if (IsInfinite(group))
            return false;

        if (data.StartTime <= 0)
            return false;

        var start = DateTimeOffset.FromUnixTimeSeconds(data.StartTime);
        var now = DateTimeOffset.UtcNow;

        return now - start >= group.Duration;
    }

    /// Lấy offer hiện tại có thể mua
    public string GetAvailableOffer(string groupId)
    {
        var data = state.Get(groupId);

        if (data == null || !data.IsActivated)
            return null;

        var group = config.Get(groupId);

        if (group == null)
            return null;

        if (IsExpired(data, group))
        {
            events?.OnGroupExpired(data);
            return null;
        }

        switch (group.Type)
        {
            case OfferGroupType.ChainDeals:

                if (data.CurrentIndex >= group.OfferIds.Count)
                    return null;

                return group.OfferIds[data.CurrentIndex];

            case OfferGroupType.OnlyOnePurchase:
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

    /// Kiểm tra có thể mua
    public OfferPurchaseError CanPurchase(string groupId, string offerId)
    {
        if (string.IsNullOrEmpty(groupId) || string.IsNullOrEmpty(offerId))
            return OfferPurchaseError.OfferNotFound;

        var data = state.Get(groupId);

        if (data == null)
            return OfferPurchaseError.OfferNotActive;

        var group = config.Get(groupId);

        if (group == null)
            return OfferPurchaseError.OfferNotFound;

        if (!data.IsActivated)
            return OfferPurchaseError.OfferNotActive;

        if (IsExpired(data, group))
            return OfferPurchaseError.OfferExpired;

        var availableOffer = GetAvailableOffer(groupId);

        if (availableOffer == null)
            return OfferPurchaseError.OfferNotActive;

        if (availableOffer != offerId)
            return OfferPurchaseError.PurchaseNotAllowed;

        return OfferPurchaseError.None;
    }

    /// Player mua offer trong group
    public PurchaseOfferGroupResponse Purchase(string groupId, string offerId)
    {
        var data = state.Get(groupId);

        if (data == null)
        {
            events?.OnGroupPurchaseFailed(null, offerId, OfferPurchaseError.OfferNotFound);

            return PurchaseOfferGroupResponse.Fail(
                OfferPurchaseError.OfferNotFound,
                groupId,
                offerId
            );
        }

        var check = CanPurchase(groupId, offerId);

        if (check != OfferPurchaseError.None)
        {
            events?.OnGroupPurchaseFailed(data, offerId, check);

            return PurchaseOfferGroupResponse.Fail(
                check,
                groupId,
                offerId,
                data
            );
        }

        var group = config.Get(groupId);

        ApplyPurchase(group, data, offerId);

        Save();

        events?.OnGroupOfferPurchased(data, offerId);

        if (IsGroupCompleted(group, data))
        {
            events?.OnGroupCompleted(data);
        }

        return PurchaseOfferGroupResponse.SuccessResult(groupId, offerId, data);
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

    private bool IsGroupCompleted(
        GameOfferGroupConfigData group,
        GameOfferGroupRuntimeData data)
    {
        switch (group.Type)
        {
            case OfferGroupType.ChainDeals:
                return data.CurrentIndex >= group.OfferIds.Count;

            case OfferGroupType.OnlyOnePurchase:
                return data.PurchasedOffers.Count > 0;

            case OfferGroupType.PurchaseEachOfferOnce:
                return data.PurchasedOffers.Count >= group.OfferIds.Count;
        }

        return false;
    }

    /// Remaining time của group
    public int GetRemainingTime(string groupId)
    {
        var runtime = state.Get(groupId);

        if (runtime == null)
            return 0;

        var group = config.Get(groupId);

        if (group == null)
            return 0;

        if (!runtime.IsActivated)
            return 0;

        if (IsInfinite(group))
            return int.MaxValue; // Infinity

        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        var endTime = runtime.StartTime + (long)group.Duration.TimeSpan.TotalSeconds;

        var remaining = endTime - now;

        return remaining > 0 ? (int)remaining : 0;
    }
}