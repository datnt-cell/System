using R3;

namespace CurrencySystem.Application
{
    public interface ICurrencyEvents
    {
        Observable<CurrencyEvent> Stream { get; }
    }

    public class CurrencyEvents : ICurrencyEvents
    {
        private readonly Subject<CurrencyEvent> _events = new();

        public Observable<CurrencyEvent> Stream => _events.AsObservable();

        public void Publish(CurrencyEvent evt)
        {
            _events.OnNext(evt);
        }
    }
}