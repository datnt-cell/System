using System;
using ConditionEngine.Domain;
using GameEventModule.Domain;

namespace GameEventModule.Application
{
    /// <summary>
    /// Scheduler xử lý start / stop logic cho GameEvent
    /// </summary>
    public class GameEventScheduler
    {
        public void Tick(
            GameEventRuntime runtime,
            IConditionContext context,
            DateTime now)
        {
            var gameEvent = runtime.Event;
            var state = runtime.State;

            if (!state.IsActive)
            {
                TryStartEvent(gameEvent, state, context, now);
            }
            else
            {
                TryEndEvent(gameEvent, state, context, now);
            }
        }

        // =========================
        // START
        // =========================

        private void TryStartEvent(
            GameEvent gameEvent,
            GameEventState state,
            IConditionContext context,
            DateTime now)
        {
            if (state.CooldownEndTime > now)
                return;

            if (!gameEvent.CanStart(context))
                return;

            state.IsActive = true;
            state.StartTime = now;

            if (gameEvent.FinishPolicy.FinishType == EventFinishType.Duration)
            {
                state.EndTime = now + gameEvent.FinishPolicy.Duration;
            }
            else
            {
                state.EndTime = DateTime.MinValue;
            }
        }

        // =========================
        // END
        // =========================

        private void TryEndEvent(
            GameEvent gameEvent,
            GameEventState state,
            IConditionContext context,
            DateTime now)
        {
            switch (gameEvent.FinishPolicy.FinishType)
            {
                case EventFinishType.Duration:

                    if (now >= state.EndTime)
                    {
                        StopEvent(gameEvent, state, now);
                    }

                    break;

                case EventFinishType.Condition:

                    if (!gameEvent.CanStart(context))
                    {
                        StopEvent(gameEvent, state, now);
                    }

                    break;
            }
        }

        // =========================
        // STOP
        // =========================

        private void StopEvent(
            GameEvent gameEvent,
            GameEventState state,
            DateTime now)
        {
            state.IsActive = false;

            state.CooldownEndTime = now + gameEvent.FinishPolicy.Cooldown;
        }
    }
}