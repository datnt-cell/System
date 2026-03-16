using R3;
using StoreSystem.Domain;

namespace StoreSystem.Infrastructure
{
    public class StoreEvents : IStoreEvents
    {
        private readonly Subject<StoreEvent> _events = new();

        public Observable<StoreEvent> Stream => _events.AsObservable();

        public void Publish(StoreEvent evt)
        {
            _events.OnNext(evt);
        }
    }
}