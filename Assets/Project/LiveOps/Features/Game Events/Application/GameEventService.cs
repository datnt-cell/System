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

        // Attachment executors
        private readonly IGameEventAttachmentExecutor _attachmentExecutor;

        private readonly List<GameEventRuntime> _events = new();

        public IReadOnlyList<GameEventRuntime> Events => _events;

        public GameEventService(
            IGameEventRepository repository,
            IGameEventConfigProvider configProvider,
            GameEventScheduler scheduler,
            IGameEventAttachmentExecutor attachmentExecutor)
        {
            _repository = repository;
            _configProvider = configProvider;
            _scheduler = scheduler;
            _attachmentExecutor = attachmentExecutor;

            Initialize();
        }

        // =========================
        // INIT
        // =========================

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

        // =========================
        // UPDATE LOOP
        // =========================

        public void Tick(IConditionContext context, DateTime now)
        {
            foreach (var runtime in _events)
            {
                bool wasActive = runtime.State.IsActive;

                _scheduler.Tick(runtime, context, now);

                bool isActive = runtime.State.IsActive;

                // Event vừa start
                if (!wasActive && isActive)
                {
                    OnEventStarted(runtime);
                }
            }

            SaveStates();
        }

        // =========================
        // EVENT START
        // =========================

        private void OnEventStarted(GameEventRuntime runtime)
        {
            var attachment = runtime.Event.Attachment;

            if (attachment == null)
                return;

            _attachmentExecutor.Execute(attachment);
        }

        // =========================
        // SAVE
        // =========================

        private void SaveStates()
        {
            foreach (var runtime in _events)
            {
                _repository.SaveState(runtime.Event.Id, runtime.State);
            }
        }
    }
}