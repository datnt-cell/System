using UnityEngine;
using DesignPatterns;
using UniRx;
using System.Collections;

public partial class GameManager : SingletonPersistent<GameManager>
{
    [SerializeField] SettingModel m_SettingModel;

    [Header("Manager")]
    AdsManager AdsManager;
    [SerializeField] ShopModelView m_ShopModelView;

    IEnumerator Start()
    {
        yield return null; // frame đầu render

        InitLight();

        yield return null;
        yield return null;

        InitModels();

        yield return null;

        InitViews();

        yield return null;

        InitSystem();

        yield return DelayFrames(5);

        yield return null;

        Creator.Director.RunScene(DGameController.SCENE_NAME);
    }

    IEnumerator DelayFrames(int frames)
    {
        for (int i = 0; i < frames; i++)
            yield return null;
    }

    void InitLight()
    {

    }

    void InitModels()
    {
        m_SettingModel = new SettingModel();
    }

    void InitViews()
    {
        m_ShopModelView?.Initialize();
    }

    void InitSystem()
    {
        AdsManager.Initialize();
    }
}
