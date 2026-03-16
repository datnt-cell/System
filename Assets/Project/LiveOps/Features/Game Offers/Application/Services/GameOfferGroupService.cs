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

        if (data == null)
            return;

        foreach (var group in data)
        {
            state.Add(group);
        }
    }

    /// <summary>
    /// Save runtime data
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

        if (data.IsActivated)
            return;

        data.IsActivated = true;
        data.StartTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        Save();
    }

    /// <summary>
    /// Kiểm tra group đã hết hạn chưa
    /// </summary>
    private bool IsExpired(GameOfferGroupRuntimeData data, GameOfferGroupConfigData group)
    {
        if (!data.IsActivated)
            return false;

        if (group.Duration <= TimeSpan.Zero)
            return false;

        if (data.StartTime <= 0)
            return false;

        var start = DateTimeOffset.FromUnixTimeSeconds(data.StartTime);
        var now = DateTimeOffset.UtcNow;

        return now - start >= group.Duration;
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

        // check expiration
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

    /// <summary>
    /// Kiểm tra có thể mua offer trong group
    /// </summary>
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

    /// <summary>
    /// Player mua offer trong group
    /// </summary>
    public PurchaseOfferGroupResponse Purchase(string groupId, string offerId)
    {
        var data = state.Get(groupId);

        if (data == null)
        {
            events?.OnGroupPurchaseFailed(groupId, offerId, "Group not found");

            return PurchaseOfferGroupResponse.Fail(
                OfferPurchaseError.OfferNotFound,
                groupId,
                offerId
            );
        }

        var group = config.Get(groupId);

        if (group == null)
        {
            events?.OnGroupPurchaseFailed(groupId, offerId, "Config not found");

            return PurchaseOfferGroupResponse.Fail(
                OfferPurchaseError.OfferNotFound,
                groupId,
                offerId,
                data
            );
        }

        var check = CanPurchase(groupId, offerId);

        if (check != OfferPurchaseError.None)
        {
            events?.OnGroupPurchaseFailed(groupId, offerId, check.ToString());

            return PurchaseOfferGroupResponse.Fail(
                check,
                groupId,
                offerId,
                data
            );
        }

        ApplyPurchase(group, data, offerId);

        Save();

        events?.OnGroupOfferPurchased(groupId, offerId);

        return PurchaseOfferGroupResponse.SuccessResult(groupId, offerId, data);
    }

    /// <summary>
    /// Áp dụng logic purchase cho group
    /// </summary>
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

                data.PurchasedOffers.Add(offerId);
                break;

            case OfferGroupType.PurchaseEachOfferOnce:

                data.PurchasedOffers.Add(offerId);
                break;
        }
    }

    public int GetRemainingTime(string offerId)
    {
        var runtime = state.Get(offerId);

        if (runtime == null)
            return 0;

        var offer = config.Get(offerId);

        if (offer == null)
            return 0;

        if (!runtime.IsActivated)
            return 0;

        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        var endTime = runtime.StartTime + (long)offer.Duration.TimeSpan.TotalSeconds;

        var remaining = endTime - now;

        return remaining > 0 ? (int)remaining : 0;
    }
}