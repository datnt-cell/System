using System;
using ConditionEngine.Domain;
using GameEventModule.Domain;

namespace GameEventModule.Application
{
    public class GameEventScheduler
    {
        private static readonly TimeSpan DefaultTickInterval = TimeSpan.FromSeconds(1);

        public void Tick(
            GameEventRuntime runtime,
            IConditionContext context,
            DateTime now)
        {
            var gameEvent = runtime.Event;
            var state = runtime.State;

            if (state.NextCheckTime > now)
                return;

            if (state.IsActive)
            {
                TryEndEvent(gameEvent, state, context, now);
            }
            else
            {
                TryStartEvent(gameEvent, state, context, now);
            }

            state.NextCheckTime = now + DefaultTickInterval;
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
            if (state.IsInCooldown(now))
                return;

            if (!gameEvent.CanStart(context))
                return;

            DateTime endTime;

            if (gameEvent.FinishPolicy.IsDuration())
            {
                var duration = gameEvent.FinishPolicy.Duration;

                endTime = duration <= TimeSpan.Zero
                    ? DateTime.MaxValue
                    : now + duration;
            }
            else
            {
                endTime = DateTime.MaxValue;
            }

            state.Start(now, endTime);
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
            if (gameEvent.FinishPolicy.IsDuration())
            {
                if (now >= state.EndTime)
                {
                    state.Stop(now, gameEvent.FinishPolicy.Cooldown);
                }

                return;
            }

            if (gameEvent.FinishPolicy.IsCondition())
            {
                if (!gameEvent.CanStart(context))
                {
                    state.Stop(now, gameEvent.FinishPolicy.Cooldown);
                }
            }
        }
    }
}