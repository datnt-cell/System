using GameEventModule.Domain;

namespace GameEventModule.Application
{
    public class GameEventEvent
    {
        public GameEventEventType Type;

        public string EventId;

        public GameEventRuntime Runtime;

        public static GameEventEvent Started(GameEventRuntime runtime)
        {
            return new GameEventEvent
            {
                Type = GameEventEventType.Started,
                EventId = runtime.Event.Id.Value,
                Runtime = runtime
            };
        }

        public static GameEventEvent Stopped(GameEventRuntime runtime)
        {
            return new GameEventEvent
            {
                Type = GameEventEventType.Stopped,
                EventId = runtime.Event.Id.Value,
                Runtime = runtime
            };
        }

        public static GameEventEvent Tick(GameEventRuntime runtime)
        {
            return new GameEventEvent
            {
                Type = GameEventEventType.Tick,
                EventId = runtime.Event.Id.Value,
                Runtime = runtime
            };
        }

        public static GameEventEvent ForcedStart(string id)
        {
            return new GameEventEvent
            {
                Type = GameEventEventType.ForcedStart,
                EventId = id
            };
        }

        public static GameEventEvent ForcedStop(string id)
        {
            return new GameEventEvent
            {
                Type = GameEventEventType.ForcedStop,
                EventId = id
            };
        }

        public static GameEventEvent CooldownReset(string id)
        {
            return new GameEventEvent
            {
                Type = GameEventEventType.CooldownReset,
                EventId = id
            };
        }
    }
}