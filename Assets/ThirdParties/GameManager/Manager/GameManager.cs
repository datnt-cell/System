using UnityEngine;
using DesignPatterns;
using UniRx;
using System.Collections;

public partial class GameManager : SingletonPersistent<GameManager>
{
    [SerializeField] SettingModel m_SettingModel;
    [SerializeField] AdsModel m_AdsModel;


    [Header("ModelView")]
    [SerializeField] AdsModelView m_AdsModelView;
    [SerializeField] ShopModelView m_ShopModelView;

    public ShopModelView GetShopModelView() => m_ShopModelView;
    public AdsModelView GetAdsModelView() => m_AdsModelView;

    public SettingModel GetSettingData() => m_SettingModel;
    public AdsModel GetAdsData() => m_AdsModel;

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
        m_AdsModel = new AdsModel();
    }

    void InitViews()
    {
        m_AdsModelView?.Initialize();
        m_ShopModelView?.Initialize();
    }

    void InitSystem()
    {

    }
}
