using System;
using System.Collections.Generic;
using GameOfferSystem.Domain;
using GameOfferSystem.Infrastructure;

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

    private void Load()
    {
        var data = repository.Load();

        if (data == null)
            return;

        foreach (var offer in data)
        {
            state.Add(offer);
        }
    }

    private void Save()
    {
        repository.Save(new List<GameOfferRuntimeData>(state.ActiveOffers));
    }

    /// <summary>
    /// Activate offer
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
            StartTime = offerConfig.WaitForActivation
                ? 0
                : DateTimeOffset.UtcNow.ToUnixTimeSeconds(),

            PurchasedCount = 0,
            IsActivated = !offerConfig.WaitForActivation
        };

        state.Add(runtime);

        Save();

        events?.OnOfferActivated(runtime);
    }

    /// <summary>
    /// Player thấy offer → bắt đầu timer
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
    /// Kiểm tra có thể mua
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
            events?.OnOfferPurchaseFailed(offerId, check);
            return PurchaseOfferResponse.Fail(check);
        }

        var data = state.Get(offerId);

        data.PurchasedCount++;

        Save();

        events?.OnOfferPurchased(data);

        return PurchaseOfferResponse.SuccessResult(data);
    }

    /// <summary>
    /// Lấy danh sách offer active
    /// </summary>
    public List<GameOfferRuntimeData> GetActiveOffers()
    {
        List<GameOfferRuntimeData> result = new();
        List<string> expiredOffers = new();

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
                if (!runtime.ExpiredHandled)
                {
                    runtime.ExpiredHandled = true;

                    events?.OnOfferDeactivated(
                        runtime,
                        runtime.PurchasedCount > 0
                    );
                }

                expiredOffers.Add(runtime.OfferId);
            }
        }

        // cleanup expired
        foreach (var id in expiredOffers)
        {
            state.Remove(id);
        }

        if (expiredOffers.Count > 0)
            Save();

        return result;
    }

    /// <summary>
    /// Lấy runtime data của offer
    /// </summary>
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