using UnityEngine;

public class GameOfferEvents : IGameOfferEvents
{
    public void OnOfferActivated(GameOfferRuntimeData offer)
    {
        Debug.Log("Offer Activated: " + offer.OfferId);
    }

    public void OnOfferDeactivated(GameOfferRuntimeData offer, bool wasPurchased)
    {
        Debug.Log("Offer Expired: " + offer.OfferId);
    }

    public void OnOfferPurchased(GameOfferRuntimeData offer)
    {
        Debug.Log("Offer Purchased: " + offer.OfferId);
    }

    public void OnOfferPurchaseFailed(GameOfferRuntimeData offer, string error)
    {
        Debug.LogWarning($"Purchase Failed: {error}");
    }
}