using UnityEngine;

/// <summary>
/// Manager là entry point của Unity.
/// Giữ reference Presenter.
/// </summary>
public class IAPManager : MonoBehaviour
{
    [SerializeField] private IAPLoading view;

    public IAPPresenter Presenter { get; private set; }

    public void Initialize()
    {
        IAPInstaller installer = new IAPInstaller();
        Presenter = installer.Install(view);
    }

    private void OnDestroy()
    {
        Presenter?.Dispose();
    }
}