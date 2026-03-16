using System;
using GameOfferSystem.Infrastructure;
using R3;

public class GameOfferGroupEvents : IGameOfferGroupEvents
{
    private readonly Subject<GameOfferGroupRuntimeData> groupActivated = new();
    private readonly Subject<(string groupId, string offerId)> groupOfferPurchased = new();
    private readonly Subject<(string groupId, string offerId, string reason)> groupPurchaseFailed = new();
    private readonly Subject<string> groupCompleted = new();
    private readonly Subject<GameOfferGroupRuntimeData> groupExpired = new();

    public Observable<GameOfferGroupRuntimeData> GroupActivated => groupActivated;
    public Observable<(string groupId, string offerId)> GroupOfferPurchased => groupOfferPurchased;
    public Observable<(string groupId, string offerId, string reason)> GroupPurchaseFailed => groupPurchaseFailed;
    public Observable<string> GroupCompleted => groupCompleted;
    public Observable<GameOfferGroupRuntimeData> GroupExpired => groupExpired;

    public void OnGroupActivated(GameOfferGroupRuntimeData data)
    {
        groupActivated.OnNext(data);
    }

    public void OnGroupOfferPurchased(string groupId, string offerId)
    {
        groupOfferPurchased.OnNext((groupId, offerId));
    }

    public void OnGroupPurchaseFailed(string groupId, string offerId, string reason)
    {
        groupPurchaseFailed.OnNext((groupId, offerId, reason));
    }

    public void OnGroupCompleted(string groupId)
    {
        groupCompleted.OnNext(groupId);
    }

    public void OnGroupExpired(GameOfferGroupRuntimeData data)
    {
        groupExpired.OnNext(data);
    }
}