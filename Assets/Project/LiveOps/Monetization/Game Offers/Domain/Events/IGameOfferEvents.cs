using GameOfferSystem.Domain;

public enum GameOfferEventType
{
    Activated,
    Deactivated,
    Purchased,
    PurchaseFailed,
    Expired
}

public class GameOfferEvent
{
    public GameOfferEventType Type;

    public string OfferId;

    public GameOfferRuntimeData Offer;

    public OfferPurchaseError Error;

    public bool WasPurchased;
}

/// <summary>
/// Interface sự kiện của Game Offer System
/// Tương tự cách Balancy gửi sự kiện LiveOps
/// </summary>
public interface IGameOfferEvents
{
    /// Offer được kích hoạt
    void OnOfferActivated(GameOfferRuntimeData offer);

    /// Offer bị deactivate
    void OnOfferDeactivated(GameOfferRuntimeData offer, bool wasPurchased);

    /// Player mua thành công
    void OnOfferPurchased(GameOfferRuntimeData offer);

    /// Player mua thất bại
    void OnOfferPurchaseFailed(GameOfferRuntimeData offer, OfferPurchaseError error);

    /// Offer hết hạn
    void OnOfferExpired(GameOfferRuntimeData offer);
}