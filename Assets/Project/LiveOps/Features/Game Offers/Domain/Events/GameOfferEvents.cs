using R3;
using GameOfferSystem.Domain;

public class GameOfferEvents : IGameOfferEvents
{
    private readonly Subject<GameOfferRuntimeData> _offerActivated = new();
    private readonly Subject<(GameOfferRuntimeData offer, bool wasPurchased)> _offerDeactivated = new();
    private readonly Subject<GameOfferRuntimeData> _offerPurchased = new();
    private readonly Subject<(string offerId, OfferPurchaseError error)> _offerPurchaseFailed = new();

    public Observable<GameOfferRuntimeData> OfferActivated => _offerActivated;

    public Observable<(GameOfferRuntimeData offer, bool wasPurchased)> OfferDeactivated => _offerDeactivated;

    public Observable<GameOfferRuntimeData> OfferPurchased => _offerPurchased;

    public Observable<(string offerId, OfferPurchaseError error)> OfferPurchaseFailed => _offerPurchaseFailed;

    public void OnOfferActivated(GameOfferRuntimeData offer)
    {
        _offerActivated.OnNext(offer);
    }

    public void OnOfferDeactivated(GameOfferRuntimeData offer, bool wasPurchased)
    {
        _offerDeactivated.OnNext((offer, wasPurchased));
    }

    public void OnOfferPurchased(GameOfferRuntimeData offer)
    {
        _offerPurchased.OnNext(offer);
    }

    public void OnOfferPurchaseFailed(string offerId, OfferPurchaseError error)
    {
        _offerPurchaseFailed.OnNext((offerId, error));
    }
}