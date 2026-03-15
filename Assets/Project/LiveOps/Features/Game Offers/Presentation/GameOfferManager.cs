using UnityEngine;
using GameOfferSystem.Installer;

/// <summary>
/// Entry point của Game Offer System trong Scene.
/// 
/// Manager chịu trách nhiệm:
/// - Khởi tạo hệ thống thông qua Installer
/// - Giữ reference tới Presenter / ViewModel / Services
/// </summary>
public class GameOfferManager : MonoBehaviour
{
    /// <summary>
    /// Service xử lý logic Offer
    /// </summary>
    public GameOfferService OfferService { get; private set; }

    /// <summary>
    /// Service xử lý logic Offer Group
    /// </summary>
    public GameOfferGroupService GroupService { get; private set; }

    private GameOfferInstaller _installer;

    /// <summary>
    /// Khởi tạo toàn bộ Game Offer System.
    /// Chỉ được gọi một lần.
    /// </summary>
    public void Initialize()
    {
        _installer = new GameOfferInstaller();

        var result = _installer.Install();
;
        OfferService = result.OfferService;
        GroupService = result.GroupService;
    }
}