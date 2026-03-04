/// <summary>
/// Interface định nghĩa luật hiển thị quảng cáo.
/// Có thể thay đổi bằng RemoteConfig hoặc LiveOps.
/// </summary>
public interface IAdsPolicy
{
    /// <summary>
    /// Kiểm tra có được phép hiển thị Interstitial không
    /// </summary>
    bool CanShowInterstitial(int level, float timeSinceLastAd);
}

/// <summary>
/// Luật mặc định cho quảng cáo.
/// Có thể cấu hình bằng constructor.
/// </summary>
public class DefaultAdsPolicy : IAdsPolicy
{
    private readonly int _minLevelToShowAds;
    private readonly float _cooldownSeconds;

    /// <summary>
    /// Constructor cho phép truyền tham số cấu hình
    /// </summary>
    public DefaultAdsPolicy(int minLevelToShowAds, float cooldownSeconds)
    {
        _minLevelToShowAds = minLevelToShowAds;
        _cooldownSeconds = cooldownSeconds;
    }

    /// <summary>
    /// Kiểm tra có được phép hiển thị Interstitial không
    /// </summary>
    public bool CanShowInterstitial(int level, float timeSinceLastAd)
    {
        if (level < _minLevelToShowAds)
            return false;

        if (timeSinceLastAd < _cooldownSeconds)
            return false;

        return true;
    }
}