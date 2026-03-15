using R3;

public enum AdType
{
    Interstitial,
    Rewarded,
    Custom
}

public class AdTypeState
{
    /// <summary>
    /// Tổng revenue của loại quảng cáo
    /// </summary>
    public ReactiveProperty<double> Revenue { get; }
        = new(0);

    /// <summary>
    /// Tổng số lần hiển thị
    /// </summary>
    public ReactiveProperty<int> Count { get; }
        = new(0);

    /// <summary>
    /// Revenue hôm nay
    /// </summary>
    public ReactiveProperty<double> RevenueToday { get; }
        = new(0);

    /// <summary>
    /// Số lần hiển thị hôm nay
    /// </summary>
    public ReactiveProperty<int> CountToday { get; }
        = new(0);

    public void AddImpression(double revenue)
    {
        Count.Value++;
        CountToday.Value++;

        Revenue.Value += revenue;
        RevenueToday.Value += revenue;
    }

    public void ResetToday()
    {
        RevenueToday.Value = 0;
        CountToday.Value = 0;
    }

    public void Restore(int count, double revenue)
    {
        Count.Value = count;
        Revenue.Value = revenue;
    }
}