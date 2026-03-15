using GameEventModule.Domain;

namespace GameEventModule.Application
{
    /// <summary>
    /// Runtime container của GameEvent
    /// </summary>
    public class GameEventRuntime
    {
        public GameEvent Event { get; }

        public GameEventState State { get; }

        public GameEventRuntime(GameEvent gameEvent, GameEventState state)
        {
            Event = gameEvent;
            State = state;
        }
    }
}