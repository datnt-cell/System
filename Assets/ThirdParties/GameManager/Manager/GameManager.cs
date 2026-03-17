using UnityEngine;
using DesignPatterns;
using System.Collections;
using StoreSystem.Presentation;
using IAPModule;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;
using PlayerSystem.Presentation;
using ConditionEngine;
using BattlePassModule;
using System;

/// <summary>
/// GameManager là entry point của game.
/// Chịu trách nhiệm khởi tạo toàn bộ hệ thống global
/// như Ads, IAP, Currency, Store, Settings...
///
/// Sử dụng SingletonPersistent để tồn tại xuyên scene.
/// </summary>
public partial class GameManager : SingletonPersistent<GameManager>
{
    // =========================
    // GAME SYSTEMS
    // =========================

    [Title("🎮 Game Systems")]

    [BoxGroup("Game Systems")]
    [PropertyOrder(0)]
    [LabelText("Settings")]
    public SettingManager SettingManager;

    [BoxGroup("Game Systems")]
    [PropertyOrder(1)]
    [LabelText("Player")]
    public PlayerManager Player;

    [BoxGroup("Game Systems")]
    [PropertyOrder(2)]
    [LabelText("Currency")]
    public CurrencyManager Currency;

    [BoxGroup("Game Systems")]
    [PropertyOrder(3)]
    [LabelText("Conditions")]
    public ConditionManager Conditions;

    [BoxGroup("Game Systems")]
    [PropertyOrder(4)]
    [LabelText("Game Events")]
    public GameEventManager GameEvents;

    [BoxGroup("Game Systems")]
    [PropertyOrder(4)]
    [LabelText("Gacha")]
    public GachaManager Gacha;

    [BoxGroup("Game Systems")]
    [PropertyOrder(4)]
    [LabelText("Battle Pass")]
    public BattlePassManager BattlePass;

    // =========================
    // MONETIZATION
    // =========================

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
    [LabelText("Offers")]
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
        await Step(SettingManager.Initialize);
        await Step(Player.Initialize);
        await Step(Currency.Initialize);

        await StepAsync(AdsManager.Initialize);
        await StepAsync(IAPManager.Initialize);

        await Step(Conditions.Initialize);
        await Step(() => Store.Initialize(Currency));
        await Step(GameOffers.Initialize);

        await Step(() => GameEvents.Initialize(Conditions.Service.GetContext()));

        await Step(Gacha.Initialize);
        await Step(BattlePass.Initialize);
    }

    async UniTask Step(Action action)
    {
        action?.Invoke();
        await UniTask.Yield();
    }

    async UniTask StepAsync(Func<UniTask> action)
    {
        if (action != null)
            await action();

        await UniTask.Yield();
    }
}