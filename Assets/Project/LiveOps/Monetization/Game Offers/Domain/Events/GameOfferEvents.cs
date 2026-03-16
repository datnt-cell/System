using R3;
using GameOfferSystem.Domain;

public class GameOfferEvents : IGameOfferEvents
{
    private readonly Subject<GameOfferEvent> _events = new();

    public Observable<GameOfferEvent> Stream => _events.AsObservable();

    public void OnOfferActivated(GameOfferRuntimeData offer)
    {
        Publish(GameOfferEventType.Activated, offer);
    }

    public void OnOfferDeactivated(GameOfferRuntimeData offer, bool wasPurchased)
    {
        _events.OnNext(new GameOfferEvent
        {
            Type = GameOfferEventType.Deactivated,
            Offer = offer,
            OfferId = offer.OfferId,
            WasPurchased = wasPurchased
        });
    }

    public void OnOfferPurchased(GameOfferRuntimeData offer)
    {
        Publish(GameOfferEventType.Purchased, offer);
    }

    public void OnOfferPurchaseFailed(GameOfferRuntimeData offer, OfferPurchaseError error)
    {
        _events.OnNext(new GameOfferEvent
        {
            Type = GameOfferEventType.PurchaseFailed,
            Offer = offer,
            OfferId = offer.OfferId,
            Error = error
        });
    }

    public void OnOfferExpired(GameOfferRuntimeData offer)
    {
        Publish(GameOfferEventType.Expired, offer);
    }

    private void Publish(GameOfferEventType type, GameOfferRuntimeData offer)
    {
        _events.OnNext(new GameOfferEvent
        {
            Type = type,
            Offer = offer,
            OfferId = offer.OfferId
        });
    }
}