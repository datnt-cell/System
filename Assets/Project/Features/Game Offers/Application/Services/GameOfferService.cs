using System;
using System.Collections.Generic;
using GameOfferSystem.Domain;
using GameOfferSystem.Infrastructure;

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
            // IsActivated = !offerConfig.WaitForActivation
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
        var data = state.Get(offerId);

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
    /// Kiểm tra có thể mua offer không
    /// </summary>
    public bool CanPurchase(string offerId)
    {
        var offer = config.Get(offerId);

        if (offer == null)
            return false;

        var data = state.Get(offerId);

        if (data == null)
            return false;

        if (!data.IsActivated)
            return false;

        if (data.IsExpired(offer.Duration))
            return false;

        if (data.PurchasedCount >= offer.Limit)
            return false;

        return true;
    }

    /// <summary>
    /// Player mua offer
    /// </summary>
    public bool Purchase(string offerId)
    {
        var data = state.Get(offerId);

        if (data == null)
        {
            events?.OnOfferPurchaseFailed(null, "Offer not found");
            return false;
        }

        if (!CanPurchase(offerId))
        {
            events?.OnOfferPurchaseFailed(data, "Purchase not allowed");
            return false;
        }

        data.PurchasedCount++;

        Save();

        // 🔔 Event
        events?.OnOfferPurchased(data);

        return true;
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