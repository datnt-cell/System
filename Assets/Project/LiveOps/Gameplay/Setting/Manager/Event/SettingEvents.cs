using R3;

public class SettingEvents : ISettingEvents
{
    private readonly Subject<SettingEvent> _events = new();

    public Observable<SettingEvent> Stream => _events.AsObservable();

    public void Publish(SettingEvent evt)
    {
        _events.OnNext(evt);
    }
}