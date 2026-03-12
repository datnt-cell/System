using UnityEngine;
using GameOfferSystem.Presentation;
using GameOfferSystem.Installer;

/// <summary>
/// Entry point của Game Offer System trong Scene.
///
/// Manager chỉ giữ reference tới các layer cần thiết
/// và khởi tạo hệ thống thông qua Installer.
/// </summary>
public class GameOfferManager : MonoBehaviour
{
    public GameOfferViewModel ViewModel { get; private set; }

    public GameOfferPresenter Presenter { get; private set; }

    public GameOfferService Service { get; private set; }

    private GameOfferInstaller _installer;

    /// <summary>
    /// Khởi tạo toàn bộ Game Offer System.
    /// </summary>
    public void Initialize()
    {
        _installer = new GameOfferInstaller();

        var result = _installer.Install();

        Presenter = result.Presenter;
        ViewModel = result.ViewModel;
        Service = result.Service;
    }
}