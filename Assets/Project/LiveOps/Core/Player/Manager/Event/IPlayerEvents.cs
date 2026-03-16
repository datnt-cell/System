using R3;

public interface IPlayerEvents
{
    Observable<PlayerEvent> Stream { get; }
}