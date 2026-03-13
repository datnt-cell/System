using System;
using GameOfferSystem.Infrastructure;

/// <summary>
/// Event dispatcher cho GameOfferGroup.
/// Các hệ khác (UI, Analytics, Gameplay) có thể subscribe.
/// </summary>
public class GameOfferGroupEvents : IGameOfferGroupEvents
{
    /// <summary>
    /// Khi group được kích hoạt
    /// </summary>
    public event Action<GameOfferGroupRuntimeData> GroupActivated;

    /// <summary>
    /// Khi player mua một offer trong group
    /// </summary>
    public event Action<string, string> GroupOfferPurchased;

    /// <summary>
    /// Khi purchase thất bại
    /// </summary>
    public event Action<string, string, string> GroupPurchaseFailed;

    /// <summary>
    /// Khi group hoàn thành (optional)
    /// </summary>
    public event Action<string> GroupCompleted;

    public void OnGroupActivated(GameOfferGroupRuntimeData data)
    {
        GroupActivated?.Invoke(data);
    }

    public void OnGroupOfferPurchased(string groupId, string offerId)
    {
        GroupOfferPurchased?.Invoke(groupId, offerId);
    }

    public void OnGroupPurchaseFailed(string groupId, string offerId, string reason)
    {
        GroupPurchaseFailed?.Invoke(groupId, offerId, reason);
    }

    public void OnGroupCompleted(string groupId)
    {
        GroupCompleted?.Invoke(groupId);
    }
}
