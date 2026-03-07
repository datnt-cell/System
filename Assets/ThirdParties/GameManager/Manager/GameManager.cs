using UnityEngine;
using DesignPatterns;
using System.Collections;
using IAPModule;

public partial class GameManager : SingletonPersistent<GameManager>
{
    [Header("Manager")]
    public AdsManager AdsManager;
    public IAPModule.IAPManager IAPManager;
    public SettingManager SettingManager;
    public CurrencyManager CurrencyManager;

    IEnumerator Start()
    {
        yield return null;

        InitSystem();

        yield return DelayFrames(5);

        yield return null;

        Creator.Director.SetRootScene(DGameController.SCENE_NAME);
    }

    IEnumerator DelayFrames(int frames)
    {
        for (int i = 0; i < frames; i++)
            yield return null;
    }

    void InitSystem()
    {
        AdsManager.Initialize();

        IAPManager.Initialize();

        SettingManager.Initialize();

        CurrencyManager.Initialize();
    }
}
