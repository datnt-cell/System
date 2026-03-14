using GameOfferSystem.Domain;

public class PurchaseOfferGroupResponse
{
    public bool Success { get; private set; }

    public OfferPurchaseError Error { get; private set; }

    public string GroupId { get; private set; }

    public string OfferId { get; private set; }

    public GameOfferGroupRuntimeData Group { get; private set; }

    public static PurchaseOfferGroupResponse SuccessResult(
        string groupId,
        string offerId,
        GameOfferGroupRuntimeData group)
    {
        return new PurchaseOfferGroupResponse
        {
            Success = true,
            Error = OfferPurchaseError.None,
            GroupId = groupId,
            OfferId = offerId,
            Group = group
        };
    }

    public static PurchaseOfferGroupResponse Fail(
        OfferPurchaseError error,
        string groupId,
        string offerId,
        GameOfferGroupRuntimeData group = null)
    {
        return new PurchaseOfferGroupResponse
        {
            Success = false,
            Error = error,
            GroupId = groupId,
            OfferId = offerId,
            Group = group
        };
    }
}