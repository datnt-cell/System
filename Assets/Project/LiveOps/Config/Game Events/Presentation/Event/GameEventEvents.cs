using R3;

namespace GameEventModule.Application
{
    public class GameEventEvents : IGameEventEvents
    {
        private readonly Subject<GameEventEvent> _events = new();

        public Observable<GameEventEvent> Stream => _events.AsObservable();

        public void Publish(GameEventEvent evt)
        {
            _events.OnNext(evt);
        }
    }
}