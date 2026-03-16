using System;
using GachaSystem.Domain.Models;

public enum GachaEventType
{
    RollStart,
    RollResult,
    RollError
}

public class GachaEvent
{
    public GachaEventType Type { get; }

    public string PoolId { get; }

    public GachaResult Result { get; }

    public Exception Exception { get; }

    public DateTime Timestamp { get; }

    private GachaEvent(
        GachaEventType type,
        string poolId = null,
        GachaResult result = null,
        Exception exception = null)
    {
        Type = type;
        PoolId = poolId;
        Result = result;
        Exception = exception;
        Timestamp = DateTime.UtcNow;
    }

    // =========================
    // FACTORY
    // =========================

    public static GachaEvent Start(string poolId)
    {
        return new GachaEvent(
            GachaEventType.RollStart,
            poolId
        );
    }

    public static GachaEvent ResultEvent(GachaResult result)
    {
        return new GachaEvent(
            GachaEventType.RollResult,
            result?.PoolId,
            result
        );
    }

    public static GachaEvent Error(string poolId, Exception e)
    {
        return new GachaEvent(
            GachaEventType.RollError,
            poolId,
            exception: e
        );
    }
}