using GameOfferSystem.Domain;

public interface IPurchaseOfferResult
{
    bool Success { get; }

    OfferPurchaseError  Error { get; }

    GameOfferRuntimeData Offer { get; }
}