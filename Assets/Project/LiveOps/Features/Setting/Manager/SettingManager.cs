using UnityEngine;

/// <summary>
/// Manager là entry point của Unity.
/// Giữ reference Presenter.
/// </summary>
public class SettingManager : MonoBehaviour
{
    public SettingPresenter Presenter { get; private set; }

    public void Initialize()
    {
        SettingInstaller installer = new SettingInstaller();
        Presenter = installer.Install();
    }

    private void OnDestroy()
    {
        Presenter?.Dispose();
    }
}
