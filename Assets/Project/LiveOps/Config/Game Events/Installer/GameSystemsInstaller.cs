using GameEventModule.Application;
using GameEventModule.Infrastructure;
using GameEventModule.Infrastructure.Repositories;
using GameOfferSystem.Domain;
using GameOfferSystem.Infrastructure;
using UnityEngine;

public class GameSystemsInstaller
{
    public GameEventService InstallGameEvents()
    {
        // =========================
        // ADAPTER
        // =========================

        var offerAdapter = new GameOfferServiceAdapter(
            GameManager.Instance.GameOffers.OfferService,
            GameManager.Instance.GameOffers.GroupService
        );

        // =========================
        // CORE SYSTEM
        // =========================

        var repository = new ES3GameEventRepository();
        var configProvider = new GameEventConfigProvider();
        var scheduler = new GameEventScheduler();

        var attachmentExecutor =
            new GameEventAttachmentExecutor(offerAdapter);

        var events = new GameEventEvents();

        return new GameEventService(
            repository,
            configProvider,
            scheduler,
            attachmentExecutor,
            events
        );
    }
}