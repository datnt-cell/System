using R3;

public interface IAdsEvents
{
    Observable<AdsEvent> Stream { get; }
}