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

    // kiểm tra offer có hết hạn chưa
    public bool IsExpired(int duration)
    {
        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        return now > StartTime + duration;
    }
}