using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public AdsView adsView;

    public AdsPresenter AdsPresenter;

    public void Initialize()
    {
        AdsInstaller adsInstaller = new AdsInstaller();
        AdsPresenter = adsInstaller.Install(adsView);
    }
}
