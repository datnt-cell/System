using R3;

public class BattlePassEvents
{
    private readonly Subject<BattlePassEvent> _events = new();

    public Observable<BattlePassEvent> Stream => _events.AsObservable();

    public void Publish(BattlePassEvent evt)
    {
        _events.OnNext(evt);
    }
}