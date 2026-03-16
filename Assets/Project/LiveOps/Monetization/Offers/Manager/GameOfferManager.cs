using UnityEngine;
using GameOfferSystem.Installer;
using GameOfferSystem.Infrastructure;

public class GameOfferManager : MonoBehaviour
{
    public GameOfferFacadeService Service { get; private set; }

    public GameOfferService OfferService { get; private set; }
    public GameOfferGroupService GroupService { get; private set; }

    public GameOfferEvents OfferEvents { get; private set; }
    public GameOfferGroupEvents GroupEvents { get; private set; }

    private GameOfferInstaller _installer;

    public void Initialize()
    {
        _installer = new GameOfferInstaller();

        var result = _installer.Install();

        OfferService = result.OfferService;
        GroupService = result.GroupService;

        OfferEvents = result.OfferEvents;
        GroupEvents = result.GroupEvents;

        Service = new GameOfferFacadeService(
            OfferService,
            GroupService
        );
    }
}