using System;
using System.Collections.Generic;
using ConditionEngine.Domain;
using GameEventModule.Domain;

namespace GameEventModule.Application
{
    /// <summary>
    /// Service quản lý toàn bộ GameEvent
    /// </summary>
    public class GameEventService
    {
        private readonly IGameEventRepository _repository;
        private readonly IGameEventConfigProvider _configProvider;
        private readonly GameEventScheduler _scheduler;

        private readonly List<GameEventRuntime> _events = new();

        public IReadOnlyList<GameEventRuntime> Events => _events;

        public GameEventService(
            IGameEventRepository repository,
            IGameEventConfigProvider configProvider,
            GameEventScheduler scheduler)
        {
            _repository = repository;
            _configProvider = configProvider;
            _scheduler = scheduler;

            Initialize();
        }

        private void Initialize()
        {
            var configs = _configProvider.GetEvents();

            foreach (var gameEvent in configs)
            {
                var state = _repository.GetState(gameEvent.Id)
                           ?? new GameEventState();

                _events.Add(new GameEventRuntime(gameEvent, state));
            }
        }

        public void Tick(IConditionContext context, DateTime now)
        {
            _scheduler.Tick(_events, context, now);

            SaveStates();
        }

        private void SaveStates()
        {
            foreach (var runtime in _events)
            {
                _repository.SaveState(runtime.Event.Id, runtime.State);
            }
        }
    }
}