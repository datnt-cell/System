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

        private readonly GameEventEvents _events;

        private readonly List<GameEventRuntime> _eventsList = new();

        public IReadOnlyList<GameEventRuntime> Events => _eventsList;

        public IGameEventEvents EventsStream => _events;

        public GameEventService(
            IGameEventRepository repository,
            IGameEventConfigProvider configProvider,
            GameEventScheduler scheduler,
            IGameEventAttachmentExecutor attachmentExecutor,
            GameEventEvents events)
        {
            _repository = repository;
            _configProvider = configProvider;
            _scheduler = scheduler;
            _attachmentExecutor = attachmentExecutor;
            _events = events;

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

                _eventsList.Add(new GameEventRuntime(gameEvent, state));
            }
        }

        // =========================
        // UPDATE LOOP
        // =========================

        public void Tick(IConditionContext context, DateTime now)
        {
            foreach (var runtime in _eventsList)
            {
                bool wasActive = runtime.State.IsActive;

                _scheduler.Tick(runtime, context, now);

                bool isActive = runtime.State.IsActive;

                _events.Publish(GameEventEvent.Tick(runtime));

                if (!wasActive && isActive)
                {
                    OnEventStarted(runtime);

                    _events.Publish(GameEventEvent.Started(runtime));
                }

                if (wasActive && !isActive)
                {
                    _events.Publish(GameEventEvent.Stopped(runtime));
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
            return _eventsList.FirstOrDefault(x => x.Event.Id.Equals(id));
        }

        public bool IsEventActive(string eventId)
        {
            var runtime = _eventsList.FirstOrDefault(x => x.Event.Id.Value == eventId);

            return runtime != null && runtime.State.IsActive;
        }

        public List<GameEventRuntime> GetActiveEvents()
        {
            return _eventsList
                .Where(x => x.State.IsActive)
                .ToList();
        }

        public bool HasEvent(string eventId)
        {
            return _eventsList.Any(x => x.Event.Id.Value == eventId);
        }

        // =========================
        // CONTROL (DEBUG / LIVEOPS)
        // =========================

        public void ForceStart(string eventId, DateTime now)
        {
            var runtime = _eventsList.FirstOrDefault(x => x.Event.Id.Value == eventId);

            if (runtime == null)
                return;

            runtime.State.IsActive = true;
            runtime.State.StartTime = now;

            OnEventStarted(runtime);

            _events.Publish(GameEventEvent.ForcedStart(eventId));

            SaveStates();
        }

        public void ForceStop(string eventId, DateTime now)
        {
            var runtime = _eventsList.FirstOrDefault(x => x.Event.Id.Value == eventId);

            if (runtime == null)
                return;

            runtime.State.IsActive = false;
            runtime.State.CooldownEndTime = now + runtime.Event.FinishPolicy.Cooldown;

            _events.Publish(GameEventEvent.ForcedStop(eventId));

            SaveStates();
        }

        public void ResetCooldown(string eventId)
        {
            var runtime = _eventsList.FirstOrDefault(x => x.Event.Id.Value == eventId);

            if (runtime == null)
                return;

            runtime.State.CooldownEndTime = DateTime.MinValue;

            _events.Publish(GameEventEvent.CooldownReset(eventId));

            SaveStates();
        }

        // =========================
        // TIME UTILITY
        // =========================

        public TimeSpan GetRemainingTime(string eventId, DateTime now)
        {
            var runtime = _eventsList.FirstOrDefault(x => x.Event.Id.Value == eventId);

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
            foreach (var runtime in _eventsList)
            {
                _repository.SaveState(runtime.Event.Id, runtime.State);
            }
        }

        // =========================
        // PROGRESS
        // =========================

        public int GetProgress(string eventId, string progressKey)
        {
            var runtime = _eventsList.FirstOrDefault(x => x.Event.Id.Value == eventId);

            if (runtime == null)
                return 0;

            return runtime.State.GetProgress(progressKey);
        }

        public void AddProgress(string eventId, string progressKey, int amount)
        {
            var runtime = _eventsList.FirstOrDefault(x => x.Event.Id.Value == eventId);

            if (runtime == null)
                return;

            runtime.State.AddProgress(progressKey, amount);

            SaveStates();
        }

        public int GetTotalProgress(string eventId)
        {
            var runtime = _eventsList.FirstOrDefault(x => x.Event.Id.Value == eventId);

            if (runtime == null)
                return 0;

            return runtime.State.GetTotalProgress();
        }
    }
}