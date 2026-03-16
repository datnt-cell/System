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

    /// Activate offer
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

    /// Player nhìn thấy offer → bắt đầu timer
    public void MarkSeen(string offerId)
    {
        var data = state.Get(offerId);

        if (data == null || data.IsActivated)
            return;

        data.IsActivated = true;
        data.StartTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        Save();
    }

    /// Kiểm tra có thể mua
    public OfferPurchaseError CanPurchase(string offerId)
    {
        var offer = config.Get(offerId);

        if (offer == null)
            return OfferPurchaseError.OfferNotFound;

        var data = state.Get(offerId);

        if (data == null || !data.IsActivated)
            return OfferPurchaseError.OfferNotActive;

        if (data.IsExpired(offer.Duration))
            return OfferPurchaseError.OfferExpired;

        if (data.PurchasedCount >= offer.Limit)
            return OfferPurchaseError.PurchaseLimitReached;

        return OfferPurchaseError.None;
    }

    /// Player mua offer
    public PurchaseOfferResponse Purchase(string offerId)
    {
        var data = state.Get(offerId);

        var check = CanPurchase(offerId);

        if (check != OfferPurchaseError.None)
        {
            if (data != null)
                events?.OnOfferPurchaseFailed(data, check);

            return PurchaseOfferResponse.Fail(check);
        }

        if (data == null)
            return PurchaseOfferResponse.Fail(OfferPurchaseError.OfferNotActive);

        data.PurchasedCount++;

        Save();

        events?.OnOfferPurchased(data);

        return PurchaseOfferResponse.SuccessResult(data);
    }

    /// Lấy danh sách offer active
    public List<GameOfferRuntimeData> GetActiveOffers()
    {
        List<GameOfferRuntimeData> result = new();
        List<string> expired = new();

        foreach (var runtime in state.ActiveOffers)
        {
            var offer = config.Get(runtime.OfferId);

            if (offer == null)
                continue;

            if (!runtime.IsExpired(offer.Duration))
            {
                result.Add(runtime);
                continue;
            }

            HandleExpiration(runtime);
            expired.Add(runtime.OfferId);
        }

        foreach (var id in expired)
            state.Remove(id);

        if (expired.Count > 0)
            Save();

        return result;
    }

    private void HandleExpiration(GameOfferRuntimeData runtime)
    {
        if (runtime.ExpiredHandled)
            return;

        runtime.ExpiredHandled = true;

        events?.OnOfferExpired(runtime);

        events?.OnOfferDeactivated(
            runtime,
            runtime.PurchasedCount > 0
        );
    }

    /// Remaining time
    public int GetRemainingTime(string offerId)
    {
        var runtime = state.Get(offerId);

        if (runtime == null || !runtime.IsActivated)
            return 0;

        var offer = config.Get(offerId);

        if (offer == null)
            return 0;

        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        var endTime = runtime.StartTime +
                      (long)offer.Duration.TimeSpan.TotalSeconds;

        var remaining = endTime - now;

        return remaining > 0 ? (int)remaining : 0;
    }
}