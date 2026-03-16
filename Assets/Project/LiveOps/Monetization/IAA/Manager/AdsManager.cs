using UnityEngine;
using Cysharp.Threading.Tasks;

public class AdsManager : MonoBehaviour
{
    [SerializeField] private LoadingView loading;

    public AdsService Service { get; private set; }
    public AdsEvents Events { get; private set; }

    public async UniTask Initialize()
    {
        AdsInstaller adsInstaller = new AdsInstaller();

        var result = await adsInstaller.Install(loading);

        Service = result.Service;
        Events = result.Events;
    }

    private void OnDestroy()
    {
        Service?.Dispose();
    }
}