/// <summary>
/// Lưu dữ liệu bằng Easy Save.
/// </summary>
public class EasySaveAdsRepository : IAdsRepository
{
    private const string KEY_INTER = "ads_inter";
    private const string KEY_REWARD = "ads_reward";
    private const string KEY_REMOVE = "ads_remove";

    public void Save(AdsState state)
    {
        ES3.Save(KEY_INTER, state.InterstitialCount.Value);
        ES3.Save(KEY_REWARD, state.RewardCount.Value);
        ES3.Save(KEY_REMOVE, state.IsRemoveAds.Value);
    }

    public void Load(AdsState state)
    {
        int inter = ES3.Load(KEY_INTER, 0);
        int reward = ES3.Load(KEY_REWARD, 0);
        bool remove = ES3.Load(KEY_REMOVE, false);

        state.Restore(inter, reward, remove);
    }
}