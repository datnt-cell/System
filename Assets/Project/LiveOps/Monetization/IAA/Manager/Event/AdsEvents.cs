using R3;

public class AdsEvents : IAdsEvents
{
    private readonly Subject<AdsEvent> _events = new();

    public Observable<AdsEvent> Stream => _events.AsObservable();

    public void Publish(AdsEvent evt)
    {
        _events.OnNext(evt);
    }
}