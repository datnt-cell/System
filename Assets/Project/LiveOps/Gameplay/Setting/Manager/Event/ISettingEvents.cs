using R3;

public interface ISettingEvents
{
    Observable<SettingEvent> Stream { get; }
}