using R3;
using GachaSystem.Application.Interfaces;

namespace GachaSystem.Infrastructure.Events
{
    public class GachaEvents : IGachaEvents
    {
        private readonly Subject<GachaEvent> _events = new();

        public Observable<GachaEvent> Stream => _events;

        public void Publish(GachaEvent evt)
        {
            _events.OnNext(evt);
        }
    }
}