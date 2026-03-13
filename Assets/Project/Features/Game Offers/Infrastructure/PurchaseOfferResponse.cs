using GameOfferSystem.Domain;

public class PurchaseOfferResponse : IPurchaseOfferResult
{
    public bool Success { get; private set; }

    public OfferPurchaseError Error { get; private set; }

    public GameOfferRuntimeData Offer { get; private set; }

    public static PurchaseOfferResponse SuccessResult(GameOfferRuntimeData offer)
    {
        return new PurchaseOfferResponse
        {
            Success = true,
            Error = OfferPurchaseError.None,
            Offer = offer
        };
    }

    public static PurchaseOfferResponse Fail(OfferPurchaseError error, GameOfferRuntimeData offer = null)
    {
        return new PurchaseOfferResponse
        {
            Success = false,
            Error = error,
            Offer = offer
        };
    }
}