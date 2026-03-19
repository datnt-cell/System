using UnityEngine;
using CurrencySystem.Application;
using CurrencySystem.Installer;

public class CurrencyManager : MonoBehaviour
{
    public CurrencyService Service { get; private set; }
    public CurrencyBundleUseCase BundleUseCase { get; private set; }
    public ICurrencyEvents Events { get; private set; }
    public CurrencyBundleEvents BundleEvents { get; private set; }

    private CurrencyInstaller _installer;

    public void Initialize()
    {
        _installer = new CurrencyInstaller();
        var result = _installer.Install();
        Service = result.Service;
        BundleUseCase = result.BundleUseCase;
        Events = result.CurrencyEvents;
        BundleEvents = result.BundleEvents;
    }
}