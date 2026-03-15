using System;
using ConditionEngine.Domain;
using GameEventModule.Application;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public GameEventService Service;
    private IConditionContext _context;

    private void Awake()
    {
        var installer = new GameSystemsInstaller();

        Service = installer.InstallGameEvents();

    }

    private void Update()
    {
        if (Service == null)
            return;

        Service.Tick(
            _context,
            DateTime.UtcNow
        );
    }
}