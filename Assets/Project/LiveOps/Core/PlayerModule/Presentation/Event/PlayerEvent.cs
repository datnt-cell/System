using System;

public enum PlayerEventType
{
    LevelChanged,
    StageChanged,
    SessionAdded,
    PlayTimeAdded,
    TutorialStepChanged,
    NewUserChanged,
    Purchase,
    TrafficSourceChanged,
    TrafficCampaignChanged
}

public class PlayerEvent
{
    public PlayerEventType Type;

    public int IntValue;
    public long LongValue;
    public string StringValue;

    public static PlayerEvent Level(int level)
    {
        return new PlayerEvent
        {
            Type = PlayerEventType.LevelChanged,
            IntValue = level
        };
    }

    public static PlayerEvent Stage(int stage)
    {
        return new PlayerEvent
        {
            Type = PlayerEventType.StageChanged,
            IntValue = stage
        };
    }

    public static PlayerEvent Session(int session)
    {
        return new PlayerEvent
        {
            Type = PlayerEventType.SessionAdded,
            IntValue = session
        };
    }

    public static PlayerEvent PlayTime(int seconds)
    {
        return new PlayerEvent
        {
            Type = PlayerEventType.PlayTimeAdded,
            IntValue = seconds
        };
    }

    public static PlayerEvent Purchase(long time)
    {
        return new PlayerEvent
        {
            Type = PlayerEventType.Purchase,
            LongValue = time
        };
    }

    public static PlayerEvent TrafficSource(string source)
    {
        return new PlayerEvent
        {
            Type = PlayerEventType.TrafficSourceChanged,
            StringValue = source
        };
    }

    public static PlayerEvent TrafficCampaign(string campaign)
    {
        return new PlayerEvent
        {
            Type = PlayerEventType.TrafficCampaignChanged,
            StringValue = campaign
        };
    }
}