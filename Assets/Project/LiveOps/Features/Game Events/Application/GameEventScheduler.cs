using System;
using System.Collections.Generic;
using ConditionEngine.Domain;
using GameEventModule.Domain;

namespace GameEventModule.Application
{
    /// <summary>
    /// Scheduler kiểm tra điều kiện start / stop event
    /// </summary>
    public class GameEventScheduler
    {
        public void Tick(
            List<GameEventRuntime> events,
            IConditionContext context,
            DateTime now)
        {
            foreach (var runtime in events)
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
        }

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
        }

        private void TryEndEvent(
            GameEvent gameEvent,
            GameEventState state,
            IConditionContext context,
            DateTime now)
        {
            if (gameEvent.FinishPolicy.FinishType == EventFinishType.Duration)
            {
                if (now >= state.EndTime)
                {
                    StopEvent(gameEvent, state, now);
                }
            }
            else
            {
                if (!gameEvent.CanStart(context))
                {
                    StopEvent(gameEvent, state, now);
                }
            }
        }

        private void StopEvent(GameEvent gameEvent, GameEventState state, DateTime now)
        {
            state.IsActive = false;

            state.CooldownEndTime = now + gameEvent.FinishPolicy.Cooldown;
        }
    }
}