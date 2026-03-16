using System;
using System.Collections.Generic;
using System.Linq;
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
        // QUERY
        // =========================

        public GameEventRuntime GetEvent(GameEventId id)
        {
            return _events.FirstOrDefault(x => x.Event.Id.Equals(id));
        }

        public bool IsEventActive(string eventId)
        {
            var runtime = _events.FirstOrDefault(x => x.Event.Id.Value == eventId);

            return runtime != null && runtime.State.IsActive;
        }

        public List<GameEventRuntime> GetActiveEvents()
        {
            return _events
                .Where(x => x.State.IsActive)
                .ToList();
        }

        public bool HasEvent(string eventId)
        {
            return _events.Any(x => x.Event.Id.Value == eventId);
        }

        // =========================
        // CONTROL (DEBUG / LIVEOPS)
        // =========================

        public void ForceStart(string eventId, DateTime now)
        {
            var runtime = _events.FirstOrDefault(x => x.Event.Id.Value == eventId);

            if (runtime == null)
                return;

            runtime.State.IsActive = true;
            runtime.State.StartTime = now;

            OnEventStarted(runtime);

            SaveStates();
        }

        public void ForceStop(string eventId, DateTime now)
        {
            var runtime = _events.FirstOrDefault(x => x.Event.Id.Value == eventId);

            if (runtime == null)
                return;

            runtime.State.IsActive = false;
            runtime.State.CooldownEndTime = now + runtime.Event.FinishPolicy.Cooldown;

            SaveStates();
        }

        public void ResetCooldown(string eventId)
        {
            var runtime = _events.FirstOrDefault(x => x.Event.Id.Value == eventId);

            if (runtime == null)
                return;

            runtime.State.CooldownEndTime = DateTime.MinValue;

            SaveStates();
        }

        // =========================
        // TIME UTILITY
        // =========================

        public TimeSpan GetRemainingTime(string eventId, DateTime now)
        {
            var runtime = _events.FirstOrDefault(x => x.Event.Id.Value == eventId);

            if (runtime == null)
                return TimeSpan.Zero;

            if (!runtime.State.IsActive)
                return TimeSpan.Zero;

            if (runtime.Event.FinishPolicy.FinishType != EventFinishType.Duration)
                return TimeSpan.Zero;

            return runtime.State.EndTime - now;
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