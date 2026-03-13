using UnityEngine;
using Cysharp.Threading.Tasks;

public class AdsManager : MonoBehaviour
{
    [SerializeField] private LoadingView loading;

    public AdsPresenter AdsPresenter { get; private set; }

    public async UniTask Initialize()
    {
        AdsInstaller adsInstaller = new AdsInstaller();

        AdsPresenter = await adsInstaller.Install(loading);
    }

    private void OnDestroy()
    {
        AdsPresenter?.Dispose();
    }
}