using R3;
using GameOfferSystem.Domain;
using GameOfferSystem.Infrastructure;

/// <summary>
/// Dispatcher các sự kiện của GameOfferGroup (Reactive - R3)
/// </summary>
public class GameOfferGroupEvents : IGameOfferGroupEvents
{
    private readonly Subject<GameOfferGroupEvent> _events = new();

    public Observable<GameOfferGroupEvent> Stream => _events;

    public void OnGroupActivated(GameOfferGroupRuntimeData group)
    {
        _events.OnNext(new GameOfferGroupEvent
        {
            Type = GameOfferGroupEventType.Activated,
            Group = group,
            GroupId = group.GroupId
        });
    }

    public void OnGroupOfferPurchased(GameOfferGroupRuntimeData group, string offerId)
    {
        _events.OnNext(new GameOfferGroupEvent
        {
            Type = GameOfferGroupEventType.OfferPurchased,
            Group = group,
            GroupId = group.GroupId,
            OfferId = offerId
        });
    }

    public void OnGroupPurchaseFailed(GameOfferGroupRuntimeData group, string offerId, OfferPurchaseError error)
    {
        _events.OnNext(new GameOfferGroupEvent
        {
            Type = GameOfferGroupEventType.PurchaseFailed,
            Group = group,
            GroupId = group.GroupId,
            OfferId = offerId,
            Error = error
        });
    }

    public void OnGroupCompleted(GameOfferGroupRuntimeData group)
    {
        _events.OnNext(new GameOfferGroupEvent
        {
            Type = GameOfferGroupEventType.Completed,
            Group = group,
            GroupId = group.GroupId
        });
    }

    public void OnGroupExpired(GameOfferGroupRuntimeData group)
    {
        _events.OnNext(new GameOfferGroupEvent
        {
            Type = GameOfferGroupEventType.Expired,
            Group = group,
            GroupId = group.GroupId
        });
    }
}