using UnityEngine;
using GachaSystem.Installer;
using GachaSystem.Application.Services;
using GachaSystem.Infrastructure.Events;

public class GachaManager : MonoBehaviour
{
    public GachaService Service { get; private set; }
    public GachaEvents Events { get; private set; }

    public void Initialize()
    {
        var installer = new GachaInstaller();

        var result = installer.Install();

        Service = result.Service;
        Events = result.Events;
    }
}