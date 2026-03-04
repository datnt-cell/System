/// <summary>
/// Interface định nghĩa luật hiển thị quảng cáo.
/// Có thể thay đổi bằng RemoteConfig hoặc LiveOps.
/// </summary>
public interface IAdsPolicy
{
    /// <summary>
    /// Kiểm tra có được phép hiển thị Interstitial không
    /// </summary>
    bool CanShowInterstitial(int currentLevel, int currentSeason, float currentTime, float nextAvailableAdTime);

    float GetNextCooldown(bool isRewarded, float currentTime);
}

/// <summary>
/// Luật mặc định cho quảng cáo.
/// Có thể cấu hình bằng constructor.
/// </summary>
public class DefaultAdsPolicy : IAdsPolicy
{
    private readonly int _minLevel;
    private readonly float _minGameTime;
    private readonly float _interCooldown;
    private readonly float _rewardCooldown;
    private readonly int _seasonToStartAds;

    /// <summary>
    /// Constructor cho phép truyền tham số cấu hình
    /// </summary>
    public DefaultAdsPolicy(
        int minLevel,
        float minGameTime,
        float interCooldown,
        float rewardCooldown,
        int seasonToStartAds)
    {
        _minLevel = minLevel;
        _minGameTime = minGameTime;
        _interCooldown = interCooldown;
        _rewardCooldown = rewardCooldown;
        _seasonToStartAds = seasonToStartAds;
    }

    public bool CanShowInterstitial(
           int currentLevel,
           int currentSeason,
           float currentTime,
           float nextAvailableAdTime)
    {
        if (currentSeason <= _seasonToStartAds)
            return false;

        if (currentLevel < _minLevel)
            return false;

        if (currentTime < _minGameTime)
            return false;

        if (currentTime < nextAvailableAdTime)
            return false;

        return true;
    }

    public float GetNextCooldown(bool isRewarded, float currentTime)
    {
        float cd = isRewarded ? _rewardCooldown : _interCooldown;
        return currentTime + cd;
    }
}