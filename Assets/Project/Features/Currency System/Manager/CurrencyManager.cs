using UnityEngine;
using CurrencySystem.Presentation;
using CurrencySystem.Installer;

public class CurrencyManager : MonoBehaviour
{
    public CurrencyViewModel ViewModel { get; private set; }
    public CurrencyPresenter Presenter { get; private set; }

    private CurrencyInstaller _installer;

    public void Initialize()
    {
        _installer = new CurrencyInstaller();

        // Install trả về full dependency graph
        var result = _installer.Install();

        Presenter = result.Presenter;
        ViewModel = result.ViewModel;
    }
}