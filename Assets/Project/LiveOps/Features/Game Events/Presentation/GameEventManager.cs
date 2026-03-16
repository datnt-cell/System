using System;
using ConditionEngine.Domain;
using GameEventModule.Application;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public GameEventService Service;

    private IConditionContext _context;

    private float _timer;
    private const float TICK_INTERVAL = 1f;

    public void Initialize(IConditionContext context)
    {
        _context = context;

        var installer = new GameSystemsInstaller();
        Service = installer.InstallGameEvents();
    }

    private void Update()
    {
        if (Service == null || _context == null)
            return;

        _timer += Time.deltaTime;

        if (_timer < TICK_INTERVAL)
            return;

        _timer = 0f;

        Service.Tick(
            _context,
            DateTime.UtcNow
        );
    }
}