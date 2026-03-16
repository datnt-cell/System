using GameOfferSystem.Domain;

/// <summary>
/// Interface sự kiện của Game Offer System
/// Tương tự cách Balancy gửi sự kiện LiveOps
/// </summary>
public interface IGameOfferEvents
{
    /// <summary>
    /// Offer được kích hoạt
    /// </summary>
    void OnOfferActivated(GameOfferRuntimeData offer);

    /// <summary>
    /// Offer hết hạn hoặc bị deactivate
    /// </summary>
    void OnOfferDeactivated(GameOfferRuntimeData offer, bool wasPurchased);

    /// <summary>
    /// Player mua offer thành công
    /// </summary>
    void OnOfferPurchased(GameOfferRuntimeData offer);

    /// <summary>
    /// Player mua offer thất bại
    /// </summary>
    void OnOfferPurchaseFailed(string offerId, OfferPurchaseError error);
}