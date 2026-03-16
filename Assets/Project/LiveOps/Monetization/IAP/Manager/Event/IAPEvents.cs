using R3;
using IAPModule.Domain;

namespace IAPModule.Infrastructure
{
    public class IAPEvents : IIAPEvents
    {
        private readonly Subject<IAPEvent> _events = new();

        public Observable<IAPEvent> Stream => _events.AsObservable();

        public void Publish(IAPEvent evt)
        {
            _events.OnNext(evt);
        }
    }
}