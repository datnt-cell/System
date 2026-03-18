using UnityEngine;
using Cysharp.Threading.Tasks;
using AdsSystem.Application;

public class AdsManager : MonoBehaviour
{
    [SerializeField] private LoadingView loading;

    public AdsService Service { get; private set; }
    public IAdsEvents Events { get; private set; }

    public async UniTask Initialize()
    {
        AdsInstaller installer = new AdsInstaller();
        var result = await installer.InstallAsync(loading);

        Service = result.Service;
        Events = result.Events;
    }

    private void OnDestroy()
    {
        Service?.Dispose();
    }
}
