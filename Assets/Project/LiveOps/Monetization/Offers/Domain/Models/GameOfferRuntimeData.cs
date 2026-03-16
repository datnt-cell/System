using System;

[Serializable]
public class GameOfferRuntimeData
{
    // ID offer
    public string OfferId;

    // thời điểm offer bắt đầu
    public long StartTime;

    // số lần đã mua
    public int PurchasedCount;

    // offer đã activate chưa
    public bool IsActivated;

    public bool ExpiredHandled;

    // kiểm tra offer có hết hạn chưa
    public bool IsExpired(System.TimeSpan duration)
    {
        if (!IsActivated)
            return false;

        if (StartTime <= 0)
            return false;

        var now = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        return now - StartTime >= duration.TotalSeconds;
    }
}