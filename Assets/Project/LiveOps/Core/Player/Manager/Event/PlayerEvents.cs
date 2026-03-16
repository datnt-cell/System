using R3;

public class PlayerEvents : IPlayerEvents
{
    private readonly Subject<PlayerEvent> _events = new();

    public Observable<PlayerEvent> Stream => _events.AsObservable();

    public void Publish(PlayerEvent evt)
    {
        _events.OnNext(evt);
    }
}