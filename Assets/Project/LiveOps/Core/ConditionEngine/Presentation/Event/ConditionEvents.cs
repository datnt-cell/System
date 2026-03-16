using R3;

namespace ConditionEngine.Application
{
    public class ConditionEvents : IConditionEvents
    {
        private readonly Subject<ConditionEvent> _events = new();

        public Observable<ConditionEvent> Stream => _events.AsObservable();

        public void Publish(ConditionEvent evt)
        {
            _events.OnNext(evt);
        }
    }
}