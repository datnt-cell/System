using GameOfferSystem.Domain;
public enum GameOfferGroupEventType
{
    Activated,
    OfferPurchased,
    PurchaseFailed,
    Completed,
    Expired
}

public struct GameOfferGroupEvent
{
    public GameOfferGroupEventType Type;

    public GameOfferGroupRuntimeData Group;

    public string GroupId;

    public string OfferId;

    public OfferPurchaseError Error;
}

public interface IGameOfferGroupEvents
{
    /// Group được kích hoạt
    void OnGroupActivated(GameOfferGroupRuntimeData group);

    /// Player mua offer trong group
    void OnGroupOfferPurchased(GameOfferGroupRuntimeData group, string offerId);

    /// Player mua thất bại
    void OnGroupPurchaseFailed(GameOfferGroupRuntimeData group, string offerId, OfferPurchaseError error);

    /// Group hoàn thành
    void OnGroupCompleted(GameOfferGroupRuntimeData group);

    /// Group hết hạn
    void OnGroupExpired(GameOfferGroupRuntimeData group);
}