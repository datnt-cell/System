using System;
using System.Collections.Generic;
using GameOfferSystem.Domain;
using GameOfferSystem.Infrastructure;
using UnityEngine;

/// <summary>
/// Service quản lý toàn bộ business logic của Game Offers
/// </summary>
public class GameOfferService
{
    private readonly GameOfferState state;
    private readonly IGameOfferRepository repository;
    private readonly GameOfferGlobalConfig config;
    private readonly IGameOfferEvents events;

    public GameOfferService(
        GameOfferState state,
        IGameOfferRepository repository,
        GameOfferGlobalConfig config,
        IGameOfferEvents events)
    {
        this.state = state;
        this.repository = repository;
        this.config = config;
        this.events = events;

        Load();
    }

    /// <summary>
    /// Load runtime data từ storage
    /// </summary>
    private void Load()
    {
        var data = repository.Load();

        foreach (var offer in data)
        {
            state.Add(offer);
        }
    }

    /// <summary>
    /// Save runtime data
    /// </summary>
    private void Save()
    {
        repository.Save(new List<GameOfferRuntimeData>(state.ActiveOffers));
    }

    /// <summary>
    /// Kích hoạt một offer
    /// </summary>
    public void ActivateOffer(string offerId)
    {
        var offerConfig = config.Get(offerId);

        if (offerConfig == null)
            return;

        if (state.Contains(offerId))
            return;

        var runtime = new GameOfferRuntimeData
        {
            OfferId = offerId,
            StartTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            PurchasedCount = 0,
            IsActivated = !offerConfig.WaitForActivation
        };

        state.Add(runtime);

        Save();

        // 🔔 Event
        events?.OnOfferActivated(runtime);
    }

    /// <summary>
    /// Player nhìn thấy offer → bắt đầu timer
    /// </summary>
    public void MarkSeen(string offerId)
    {
        if (string.IsNullOrEmpty(offerId))
            return;

        var data = state.Get(offerId);

        if (data == null)
            return;

        if (data.IsActivated)
            return;

        data.IsActivated = true;
        data.StartTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        Save();
    }

    /// <summary>
    /// Kiểm tra có thể mua offer không
    /// </summary>
    public OfferPurchaseError CanPurchase(string offerId)
    {
        var offer = config.Get(offerId);

        if (offer == null)
            return OfferPurchaseError.OfferNotFound;

        var data = state.Get(offerId);

        if (data == null)
            return OfferPurchaseError.OfferNotActive;

        if (!data.IsActivated)
            return OfferPurchaseError.OfferNotActive;

        if (data.IsExpired(offer.Duration))
            return OfferPurchaseError.OfferExpired;

        if (data.PurchasedCount >= offer.Limit)
            return OfferPurchaseError.PurchaseLimitReached;

        return OfferPurchaseError.None;
    }

    /// <summary>
    /// Player mua offer
    /// </summary>
    public PurchaseOfferResponse Purchase(string offerId)
    {
        var check = CanPurchase(offerId);

        if (check != OfferPurchaseError.None)
        {
            events?.OnOfferPurchaseFailed(null, check.ToString());
            return PurchaseOfferResponse.Fail(check);
        }

        var data = state.Get(offerId);

        data.PurchasedCount++;

        Save();

        events?.OnOfferPurchased(data);

        return PurchaseOfferResponse.SuccessResult(data);
    }

    /// <summary>
    /// Lấy danh sách offer đang active
    /// </summary>
    public List<GameOfferRuntimeData> GetActiveOffers()
    {
        List<GameOfferRuntimeData> result = new();

        foreach (var runtime in state.ActiveOffers)
        {
            var offer = config.Get(runtime.OfferId);

            if (offer == null)
                continue;

            if (!runtime.IsExpired(offer.Duration))
            {
                result.Add(runtime);
            }
            else
            {
                // 🔔 Offer expired
                events?.OnOfferDeactivated(runtime, runtime.PurchasedCount > 0);
            }
        }

        return result;
    }
}