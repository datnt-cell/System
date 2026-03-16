using System;
using System.Collections.Generic;
using System.Linq;
using ConditionEngine.Domain;
using GameEventModule.Domain;

namespace GameEventModule.Application
{
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

            // sort theo priority cao -> thấp
            _eventsList.Sort((a, b) =>
                b.Event.Priority.CompareTo(a.Event.Priority));
        }

        private GameEventRuntime FindRuntime(string eventId)
        {
            return _eventsList.FirstOrDefault(x => x.Event.Id.Value == eventId);
        }

        // =========================
        // UPDATE LOOP
        // =========================

        public void Tick(IConditionContext context, DateTime now)
        {
            bool dirty = false;

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
                    dirty = true;
                }

                if (wasActive && !isActive)
                {
                    _events.Publish(GameEventEvent.Stopped(runtime));
                    _events.Publish(GameEventEvent.CooldownStarted(runtime));
                    dirty = true;
                }
            }

            if (dirty)
                SaveStates();
        }

        // =========================
        // EVENT START
        // =========================

        private void OnEventStarted(GameEventRuntime runtime)
        {
            var attachments = runtime.Event.Attachments;

            if (attachments == null || attachments.Count == 0)
                return;

            foreach (var attachment in attachments)
            {
                _attachmentExecutor.Execute(attachment);
            }
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
            var runtime = FindRuntime(eventId);
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
            return FindRuntime(eventId) != null;
        }

        // =========================
        // CONTROL (DEBUG / LIVEOPS)
        // =========================

        public void ForceStart(string eventId, DateTime now)
        {
            var runtime = FindRuntime(eventId);

            if (runtime == null)
                return;

            DateTime endTime = DateTime.MaxValue;

            if (runtime.Event.FinishPolicy.IsDuration())
            {
                var duration = runtime.Event.FinishPolicy.Duration;

                endTime = duration <= TimeSpan.Zero
                    ? DateTime.MaxValue
                    : now + duration;
            }

            runtime.State.Start(now, endTime);

            OnEventStarted(runtime);

            _events.Publish(GameEventEvent.ForcedStart(eventId));

            SaveState(runtime);
        }

        public void ForceStop(string eventId, DateTime now)
        {
            var runtime = FindRuntime(eventId);

            if (runtime == null)
                return;

            runtime.State.Stop(now, runtime.Event.FinishPolicy.Cooldown);

            _events.Publish(GameEventEvent.ForcedStop(eventId));

            SaveState(runtime);
        }

        public void ResetCooldown(string eventId)
        {
            var runtime = FindRuntime(eventId);

            if (runtime == null)
                return;

            runtime.State.CooldownEndTime = DateTime.MinValue;

            _events.Publish(GameEventEvent.CooldownReset(eventId));

            SaveState(runtime);
        }

        // =========================
        // TIME UTILITY
        // =========================

        public TimeSpan GetRemainingTime(string eventId, DateTime now)
        {
            var runtime = FindRuntime(eventId);

            if (runtime == null)
                return TimeSpan.Zero;

            if (!runtime.State.IsActive)
                return TimeSpan.Zero;

            if (!runtime.Event.FinishPolicy.IsDuration())
                return TimeSpan.Zero;

            if (runtime.State.EndTime == DateTime.MaxValue)
                return TimeSpan.MaxValue;

            var remaining = runtime.State.EndTime - now;

            return remaining > TimeSpan.Zero
                ? remaining
                : TimeSpan.Zero;
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

        private void SaveState(GameEventRuntime runtime)
        {
            _repository.SaveState(runtime.Event.Id, runtime.State);
        }

        // =========================
        // PROGRESS
        // =========================

        public int GetProgress(string eventId, string progressKey)
        {
            var runtime = FindRuntime(eventId);

            if (runtime == null)
                return 0;

            return runtime.State.GetProgress(progressKey);
        }

        public void AddProgress(string eventId, string progressKey, int amount)
        {
            var runtime = FindRuntime(eventId);

            if (runtime == null)
                return;

            runtime.State.AddProgress(progressKey, amount);

            SaveState(runtime);
        }

        public int GetTotalProgress(string eventId)
        {
            var runtime = FindRuntime(eventId);

            if (runtime == null)
                return 0;

            return runtime.State.GetTotalProgress();
        }
    }
}