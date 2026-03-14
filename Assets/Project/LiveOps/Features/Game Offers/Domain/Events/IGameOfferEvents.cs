/// <summary>
/// Interface sự kiện của Game Offer System
/// Tương tự cách Balancy gửi sự kiện LiveOps
/// </summary>
public interface IGameOfferEvents
{
    /// <summary>
    /// Offer mới được kích hoạt
    /// </summary>
    void OnOfferActivated(GameOfferRuntimeData offer);

    /// <summary>
    /// Offer bị tắt / hết hạn
    /// </summary>
    void OnOfferDeactivated(GameOfferRuntimeData offer, bool wasPurchased);

    /// <summary>
    /// Player mua offer thành công
    /// </summary>
    void OnOfferPurchased(GameOfferRuntimeData offer);

    /// <summary>
    /// Player mua thất bại
    /// </summary>
    void OnOfferPurchaseFailed(GameOfferRuntimeData offer, string error);
}