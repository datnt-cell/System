using UnityEngine;

public static class HelperLiveOps
{
    public static int GetBuildNumber()
    {
#if UNITY_ANDROID && !UNITY_EDITOR

    using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
    {
        var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        var packageManager = activity.Call<AndroidJavaObject>("getPackageManager");
        var packageName = activity.Call<string>("getPackageName");

        var packageInfo = packageManager.Call<AndroidJavaObject>("getPackageInfo", packageName, 0);

        return packageInfo.Get<int>("versionCode");
    }

#elif UNITY_IOS && !UNITY_EDITOR

    string build = UnityEngine.iOS.Device.generation.ToString();
    int.TryParse(build, out int buildNumber);
    return buildNumber;

#else

        return 1; // fallback editor

#endif
    }
}