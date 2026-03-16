public enum BattlePassEventType
{
    XPAdded,
    LevelUp,

    FreeRewardClaimed,
    PremiumRewardClaimed,

    RewardClaimedAll,

    PremiumUnlocked,

    SeasonStarted,
    SeasonEnded
}

public class BattlePassEvent
{
    public BattlePassEventType Type;

    public int Value;
    public int Level;

    public static BattlePassEvent XPAdded(int value)
    {
        return new BattlePassEvent
        {
            Type = BattlePassEventType.XPAdded,
            Value = value
        };
    }

    public static BattlePassEvent LevelUp(int level)
    {
        return new BattlePassEvent
        {
            Type = BattlePassEventType.LevelUp,
            Level = level
        };
    }

    public static BattlePassEvent FreeRewardClaimed(int level)
    {
        return new BattlePassEvent
        {
            Type = BattlePassEventType.FreeRewardClaimed,
            Level = level
        };
    }

    public static BattlePassEvent PremiumRewardClaimed(int level)
    {
        return new BattlePassEvent
        {
            Type = BattlePassEventType.PremiumRewardClaimed,
            Level = level
        };
    }

    public static BattlePassEvent RewardClaimedAll()
    {
        return new BattlePassEvent
        {
            Type = BattlePassEventType.RewardClaimedAll
        };
    }

    public static BattlePassEvent PremiumUnlocked()
    {
        return new BattlePassEvent
        {
            Type = BattlePassEventType.PremiumUnlocked
        };
    }

    public static BattlePassEvent SeasonStarted()
    {
        return new BattlePassEvent
        {
            Type = BattlePassEventType.SeasonStarted
        };
    }

    public static BattlePassEvent SeasonEnded()
    {
        return new BattlePassEvent
        {
            Type = BattlePassEventType.SeasonEnded
        };
    }
}