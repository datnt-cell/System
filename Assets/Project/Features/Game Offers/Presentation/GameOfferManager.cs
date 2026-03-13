using UnityEngine;
using GameOfferSystem.Presentation;
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
    /// ViewModel dùng cho UI binding
    /// </summary>
    public GameOfferViewModel ViewModel { get; private set; }

    /// <summary>
    /// Presenter dùng cho Gameplay/UI gọi logic
    /// </summary>
    public GameOfferPresenter Presenter { get; private set; }

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

        Presenter = result.Presenter;
        ViewModel = result.ViewModel;
        OfferService = result.OfferService;
        GroupService = result.GroupService;
    }

    [ContextMenu("TestOfferService")]
    public void TestOfferService()
    {
        Presenter.ActivateOffer("OFFER_001");
        
        var result = Presenter.PurchaseOffer("OFFER_001");

        if (!result.Success)
        {
            Debug.Log(result.Error);
            return;
        }

        Debug.Log("Purchased: " + result.Offer.OfferId);
    }
}