using R3;

public enum AdType
{
    Interstitial,
    Rewarded,
    Custom
}

/// <summary>
/// Trạng thái cho một loại quảng cáo
/// </summary>
public class AdTypeState
{
    // =========================
    // REACTIVE PROPERTIES
    // =========================

    public ReactiveProperty<double> Revenue { get; } = new(0);
    public ReactiveProperty<int> Count { get; } = new(0);
    public ReactiveProperty<double> RevenueToday { get; } = new(0);
    public ReactiveProperty<int> CountToday { get; } = new(0);

    // =========================
    // DOMAIN METHODS
    // =========================

    /// <summary>
    /// Thêm một lượt hiển thị kèm revenue
    /// </summary>
    public AdTypeResponse AddImpression(double revenue)
    {
        int oldCount = Count.Value;
        double oldRevenue = Revenue.Value;
        int oldCountToday = CountToday.Value;
        double oldRevenueToday = RevenueToday.Value;

        Count.Value++;
        CountToday.Value++;
        Revenue.Value += revenue;
        RevenueToday.Value += revenue;

        return new AdTypeResponse
        {
            Success = true,
            OldCount = oldCount,
            OldRevenue = oldRevenue,
            OldCountToday = oldCountToday,
            OldRevenueToday = oldRevenueToday,
            NewCount = Count.Value,
            NewRevenue = Revenue.Value,
            NewCountToday = CountToday.Value,
            NewRevenueToday = RevenueToday.Value,
            AddedRevenue = revenue
        };
    }

    /// <summary>
    /// Reset số liệu hôm nay
    /// </summary>
    public void ResetToday()
    {
        RevenueToday.Value = 0;
        CountToday.Value = 0;
    }

    /// <summary>
    /// Restore dữ liệu từ lưu trữ
    /// </summary>
    public void Restore(int count, double revenue, int countToday = 0, double revenueToday = 0)
    {
        Count.Value = count;
        Revenue.Value = revenue;
        CountToday.Value = countToday;
        RevenueToday.Value = revenueToday;
    }
}

/// <summary>
/// Response khi thêm impression
/// </summary>
public class AdTypeResponse
{
    public bool Success { get; set; }

    public int OldCount { get; set; }
    public double OldRevenue { get; set; }
    public int OldCountToday { get; set; }
    public double OldRevenueToday { get; set; }

    public int NewCount { get; set; }
    public double NewRevenue { get; set; }
    public int NewCountToday { get; set; }
    public double NewRevenueToday { get; set; }

    public double AddedRevenue { get; set; }
}