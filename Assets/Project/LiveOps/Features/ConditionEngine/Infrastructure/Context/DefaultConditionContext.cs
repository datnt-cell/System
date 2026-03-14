using System;
using ConditionEngine.Domain;

namespace ConditionEngine.Infrastructure
{
    /// <summary>
    /// Implementation của IConditionContext
    /// Cung cấp dữ liệu runtime cho ConditionEngine
    /// </summary>
    public class DefaultConditionContext : IConditionContext
    {
        private readonly PlayerProvider _player;
        private readonly PurchaseProvider _purchase;
        private readonly AdsProvider _ads;
        private readonly TimeProvider _time;
        private readonly InventoryProvider _inventory;
        private readonly EventProvider _event;

        public DefaultConditionContext(
            PlayerProvider player,
            PurchaseProvider purchase,
            AdsProvider ads,
            TimeProvider time,
            InventoryProvider inventory,
            EventProvider eventProvider)
        {
            _player = player;
            _purchase = purchase;
            _ads = ads;
            _time = time;
            _inventory = inventory;
            _event = eventProvider;
        }

        // =====================
        // PLAYER PROGRESS
        // =====================

        public int PlayerLevel => _player.Level;

        public int Stage => _player.Stage;

        public int SessionCount => _player.SessionCount;

        public bool IsNewUser => _player.IsNewUser;

        // =====================
        // MONETIZATION
        // =====================

        public float TotalSpend => _purchase.TotalSpend;

        public int AdsWatchCount => _ads.GetRewardedAdsWatched();

        public bool HasPurchased(string productId)
        {
            return _purchase.HasPurchased(productId);
        }

        // =====================
        // PLAYER INFO
        // =====================

        public string Country => _player.Country;

        public string PlayerSegment => _player.Segment;

        public int AppBuildVersion
        {
            get
            {
#if UNITY_ANDROID
                return GetAndroidVersionCode();
#elif UNITY_IOS
                return 0;
#else
                return 0;
#endif
            }
        }

        // =====================
        // TIME
        // =====================

        public DateTime UtcNow => _time.Now;

        public int DaysSinceInstall => _player.DaysSinceInstall;

        public int TotalPlayTimeMinutes => _player.TotalPlayTimeMinutes;

        // =====================
        // INVENTORY
        // =====================

        public bool HasItem(string itemId)
        {
            return _inventory.HasItem(itemId);
        }

        public int GetCurrency(string currencyId)
        {
            return _inventory.GetCurrency(currencyId);
        }

        // =====================
        // EVENT
        // =====================

        public int GetEventProgress(string eventId)
        {
            return _event.GetProgress(eventId);
        }

        // =====================
        // ANDROID VERSION CODE
        // =====================

        private int GetAndroidVersionCode()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            using var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            using var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            using var packageManager = activity.Call<AndroidJavaObject>("getPackageManager");
            string packageName = activity.Call<string>("getPackageName");
            using var packageInfo = packageManager.Call<AndroidJavaObject>("getPackageInfo", packageName, 0);

            return packageInfo.Get<int>("versionCode");
#else
            return 0;
#endif
        }
    }
}