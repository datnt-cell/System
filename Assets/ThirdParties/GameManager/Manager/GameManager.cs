using UnityEngine;
using DesignPatterns;
using System.Collections;
using StoreSystem.Presentation;

/// <summary>
/// GameManager là entry point của game.
/// Chịu trách nhiệm khởi tạo toàn bộ hệ thống global
/// như Ads, IAP, Currency, Store, Settings...
///
/// Sử dụng SingletonPersistent để tồn tại xuyên scene.
/// </summary>
public partial class GameManager : SingletonPersistent<GameManager>
{
    [Header("Manager")]

    /// <summary>
    /// Quản lý quảng cáo (Admob, Applovin, IronSource...)
    /// </summary>
    public AdsManager AdsManager;

    /// <summary>
    /// Quản lý In-App Purchase
    /// </summary>
    public IAPManager IAPManager;

    /// <summary>
    /// Quản lý setting game (sound, vibration...)
    /// </summary>
    public SettingManager SettingManager;

    /// <summary>
    /// Quản lý toàn bộ currency (coin, gem...)
    /// </summary>
    public CurrencyManager CurrencyManager;

    /// <summary>
    /// Quản lý các item trong store
    /// </summary>
    public StoreItemsManager StoreItemsManager;

    /// <summary>
    /// Quản lý các offer trong game (bundle, starter pack...)
    /// </summary>
    public GameOfferManager GameOfferManager;


    /// <summary>
    /// Unity Start Coroutine
    /// Khởi chạy khi GameManager được tạo.
    /// Dùng coroutine để đảm bảo thứ tự init ổn định.
    /// </summary>
    IEnumerator Start()
    {
        // đợi 1 frame để đảm bảo tất cả object trong scene đã Awake xong
        yield return null;

        // khởi tạo toàn bộ system
        InitSystem();

        // đợi vài frame để các system async initialize xong
        yield return DelayFrames(5);

        // thêm 1 frame buffer
        yield return null;

        // load scene gameplay chính
        Creator.Director.SetRootScene(DGameController.SCENE_NAME);
    }


    /// <summary>
    /// Delay một số frame nhất định
    /// Dùng khi cần chờ Unity update loop
    /// hoặc chờ các manager async khởi tạo.
    /// </summary>
    IEnumerator DelayFrames(int frames)
    {
        for (int i = 0; i < frames; i++)
            yield return null;
    }


    /// <summary>
    /// Khởi tạo toàn bộ core system của game
    /// Thứ tự init khá quan trọng vì có dependency.
    /// </summary>
    void InitSystem()
    {
        // khởi tạo Ads SDK
        AdsManager.Initialize();

        // khởi tạo hệ thống IAP
        IAPManager.Initialize();

        // load setting (sound, vibration...)
        SettingManager.Initialize();

        // load currency từ save
        CurrencyManager.Initialize();

        // khởi tạo store item (cần CurrencyManager để check tiền)
        StoreItemsManager.Initialize(CurrencyManager);

        // khởi tạo hệ thống offer (bundle, special pack)
        GameOfferManager.Initialize();
    }
}