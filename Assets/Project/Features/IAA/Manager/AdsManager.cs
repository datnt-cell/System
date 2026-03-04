using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public AdsLoading loaidng;

    public AdsPresenter AdsPresenter;

    public void Initialize()
    {
        AdsInstaller adsInstaller = new AdsInstaller();
        AdsPresenter = adsInstaller.Install(loaidng);
    }
}
