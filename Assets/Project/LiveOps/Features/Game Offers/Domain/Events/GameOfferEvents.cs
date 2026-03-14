using System;

/// <summary>
/// Dispatcher các sự kiện của hệ thống Game Offer.
/// Các hệ khác như UI, Analytics, Gameplay có thể subscribe các event này.
/// </summary>
public class GameOfferEvents : IGameOfferEvents
{
    /// <summary>
    /// Sự kiện khi một Offer được kích hoạt.
    /// </summary>
    public event Action<GameOfferRuntimeData> OfferActivated;

    /// <summary>
    /// Sự kiện khi Offer bị tắt hoặc hết hạn.
    /// bool wasPurchased = true nếu player đã mua offer trước khi tắt.
    /// </summary>
    public event Action<GameOfferRuntimeData, bool> OfferDeactivated;

    /// <summary>
    /// Sự kiện khi player mua Offer thành công.
    /// </summary>
    public event Action<GameOfferRuntimeData> OfferPurchased;

    /// <summary>
    /// Sự kiện khi player mua Offer thất bại.
    /// error: lý do thất bại (ví dụ: không đủ tiền, offer hết hạn,...)
    /// </summary>
    public event Action<GameOfferRuntimeData, string> OfferPurchaseFailed;

    /// <summary>
    /// Trigger khi Offer được kích hoạt.
    /// </summary>
    public void OnOfferActivated(GameOfferRuntimeData offer)
    {
        OfferActivated?.Invoke(offer);
    }

    /// <summary>
    /// Trigger khi Offer bị tắt hoặc hết hạn.
    /// </summary>
    public void OnOfferDeactivated(GameOfferRuntimeData offer, bool wasPurchased)
    {
        OfferDeactivated?.Invoke(offer, wasPurchased);
    }

    /// <summary>
    /// Trigger khi player mua Offer thành công.
    /// </summary>
    public void OnOfferPurchased(GameOfferRuntimeData offer)
    {
        OfferPurchased?.Invoke(offer);
    }

    /// <summary>
    /// Trigger khi player mua Offer thất bại.
    /// </summary>
    public void OnOfferPurchaseFailed(GameOfferRuntimeData offer, string error)
    {
        OfferPurchaseFailed?.Invoke(offer, error);
    }
}