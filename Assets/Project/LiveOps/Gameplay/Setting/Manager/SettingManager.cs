using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public SettingService Service { get; private set; }
    public SettingEvents Events { get; private set; }

    public void Initialize()
    {
        SettingInstaller installer = new SettingInstaller();

        var result = installer.Install();

        Service = result.Service;
        Events = result.Events;
    }

    private void OnDestroy()
    {
        Service?.Dispose();
    }
}