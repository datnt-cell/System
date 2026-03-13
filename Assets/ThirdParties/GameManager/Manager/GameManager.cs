using UnityEngine;
using DesignPatterns;
using System.Collections;
using StoreSystem.Presentation;
using IAPModule;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;

/// <summary>
/// GameManager là entry point của game.
/// Chịu trách nhiệm khởi tạo toàn bộ hệ thống global
/// như Ads, IAP, Currency, Store, Settings...
///
/// Sử dụng SingletonPersistent để tồn tại xuyên scene.
/// </summary>
public partial class GameManager : SingletonPersistent<GameManager>
{
    [Title("🎮 Game Systems")]

    [BoxGroup("Game Systems")]
    [PropertyOrder(0)]
    [LabelText("Settings")]
    public SettingManager SettingManager;

    [BoxGroup("Game Systems")]
    [PropertyOrder(1)]
    [LabelText("Currency")]
    public CurrencyManager Currency;


    [Title("💰 Monetization Systems")]

    [BoxGroup("Monetization")]
    [PropertyOrder(10)]
    [LabelText("Ads")]
    public AdsManager AdsManager;

    [BoxGroup("Monetization")]
    [PropertyOrder(11)]
    [LabelText("IAP")]
    public IAPManager IAPManager;

    [BoxGroup("Monetization")]
    [PropertyOrder(12)]
    [LabelText("Store")]
    public StoreItemsManager Store;

    [BoxGroup("Monetization")]
    [PropertyOrder(13)]
    [LabelText("Game Offers")]
    public GameOfferManager GameOffers;

    /// <summary>
    /// Unity Start Coroutine
    /// Khởi chạy khi GameManager được tạo.
    /// Dùng coroutine để đảm bảo thứ tự init ổn định.
    /// </summary>
    IEnumerator Start()
    {
        // đợi 1 frame để đảm bảo tất cả object trong scene đã Awake xong
        yield return null;

        // khởi tạo toàn bộ system và đợi hoàn thành
        yield return InitSystem().ToCoroutine();

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
    async UniTask InitSystem()
    {
        // khởi tạo Ads SDK
        await AdsManager.Initialize();

        // khởi tạo hệ thống IAP
        await IAPManager.Initialize();

        // load setting (sound, vibration...)
        SettingManager.Initialize();

        // load currency từ save
        Currency.Initialize();

        // khởi tạo store item (cần CurrencyManager để check tiền)
        Store.Initialize(Currency);

        // khởi tạo hệ thống offer (bundle, special pack)
        GameOffers.Initialize();
    }
}