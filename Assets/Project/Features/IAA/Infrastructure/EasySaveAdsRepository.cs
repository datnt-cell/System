using IAPModule.Application.Interfaces;

/// <summary>
/// Lưu dữ liệu Ads bằng Easy Save.
/// File lưu: Ads_Save.es3
/// </summary>
public class EasySaveAdsRepository : IAdsRepository
{
    private const string FILE_NAME = "Ads_Save.es3";

    private const string KEY_INTER = "ads_inter";
    private const string KEY_REWARD = "ads_reward";
    private const string KEY_REMOVE = "ads_remove";

    public void Save(AdsState state)
    {
        ES3.Save(KEY_INTER, state.InterstitialCount.Value, FILE_NAME);
        ES3.Save(KEY_REWARD, state.RewardCount.Value, FILE_NAME);
        ES3.Save(KEY_REMOVE, state.IsRemoveAds.Value, FILE_NAME);
    }

    public void Load(AdsState state)
    {
        int inter = ES3.Load(KEY_INTER, FILE_NAME, 0);
        int reward = ES3.Load(KEY_REWARD, FILE_NAME, 0);
        bool remove = ES3.Load(KEY_REMOVE, FILE_NAME, false);

        state.Restore(inter, reward, remove);
    }
}