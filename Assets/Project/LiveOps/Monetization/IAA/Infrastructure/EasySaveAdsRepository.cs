using IAPModule.Application.Interfaces;

public class EasySaveAdsRepository : IAdsRepository
{
    private const string FILE_NAME = "Ads_Save.es3";

    private const string KEY_REMOVE = "ads_remove";
    private const string KEY_TOTAL_REVENUE = "ads_total_revenue";
    private const string KEY_REVENUE_TODAY = "ads_revenue_today";

    private const string KEY_INTER_COUNT = "ads_inter_count";
    private const string KEY_INTER_REVENUE = "ads_inter_revenue";

    private const string KEY_REWARD_COUNT = "ads_reward_count";
    private const string KEY_REWARD_REVENUE = "ads_reward_revenue";

    private const string KEY_CUSTOM_COUNT = "ads_custom_count";
    private const string KEY_CUSTOM_REVENUE = "ads_custom_revenue";

    public void Save(AdsState state)
    {
        ES3.Save(KEY_REMOVE, state.IsRemoveAds.Value, FILE_NAME);

        ES3.Save(KEY_TOTAL_REVENUE, state.TotalRevenue.Value, FILE_NAME);
        ES3.Save(KEY_REVENUE_TODAY, state.RevenueToday.Value, FILE_NAME);

        SaveAdType(state, AdType.Interstitial, KEY_INTER_COUNT, KEY_INTER_REVENUE);
        SaveAdType(state, AdType.Rewarded, KEY_REWARD_COUNT, KEY_REWARD_REVENUE);
        SaveAdType(state, AdType.Custom, KEY_CUSTOM_COUNT, KEY_CUSTOM_REVENUE);
    }

    public void Load(AdsState state)
    {
        bool remove = ES3.Load(KEY_REMOVE, FILE_NAME, false);

        double totalRevenue = ES3.Load(KEY_TOTAL_REVENUE, FILE_NAME, 0d);
        double revenueToday = ES3.Load(KEY_REVENUE_TODAY, FILE_NAME, 0d);

        LoadAdType(state, AdType.Interstitial, KEY_INTER_COUNT, KEY_INTER_REVENUE);
        LoadAdType(state, AdType.Rewarded, KEY_REWARD_COUNT, KEY_REWARD_REVENUE);
        LoadAdType(state, AdType.Custom, KEY_CUSTOM_COUNT, KEY_CUSTOM_REVENUE);

        state.IsRemoveAds.Value = remove;
        state.TotalRevenue.Value = totalRevenue;
        state.RevenueToday.Value = revenueToday;
    }

    private void SaveAdType(AdsState state, AdType type, string keyCount, string keyRevenue)
    {
        var ad = state.AdTypes[type];

        ES3.Save(keyCount, ad.Count.Value, FILE_NAME);
        ES3.Save(keyRevenue, ad.Revenue.Value, FILE_NAME);
    }

    private void LoadAdType(AdsState state, AdType type, string keyCount, string keyRevenue)
    {
        int count = ES3.Load(keyCount, FILE_NAME, 0);
        double revenue = ES3.Load(keyRevenue, FILE_NAME, 0d);

        var ad = state.AdTypes[type];
        ad.Restore(count, revenue);
    }
}