using UnityEngine;
using CurrencySystem.Presentation;
using CurrencySystem.Installer;

public class CurrencyManager : MonoBehaviour
{
    private CurrencyPresenter _presenter;

    public void Initialize()
    {
        var installer = new CurrencyInstaller();
        _presenter = installer.Install();
    }
}