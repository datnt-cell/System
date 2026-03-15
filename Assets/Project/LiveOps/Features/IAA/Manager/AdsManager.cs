using UnityEngine;
using Cysharp.Threading.Tasks;

public class AdsManager : MonoBehaviour
{
    [SerializeField] private LoadingView loading;

    public AdsService Service { get; private set; }

    public async UniTask Initialize()
    {
        AdsInstaller adsInstaller = new AdsInstaller();

        Service = await adsInstaller.Install(loading);
    }

    private void OnDestroy()
    {
        Service?.Dispose();
    }
}