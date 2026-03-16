using System;
using GachaSystem.Domain.Models;
using System.Collections.Generic;

public enum GachaEventType
{
    RollStart,
    RollResult,
    RollMultipleResult,
    RollError,
    PoolRegistered
}

public class GachaEvent
{
    public GachaEventType Type { get; }

    public string PoolId { get; }

    public GachaResult Result { get; }

    public IReadOnlyList<GachaResult> Results { get; }

    public Exception Exception { get; }

    public DateTime Timestamp { get; }

    private GachaEvent(
        GachaEventType type,
        string poolId = null,
        GachaResult result = null,
        IReadOnlyList<GachaResult> results = null,
        Exception exception = null)
    {
        Type = type;
        PoolId = poolId;
        Result = result;
        Results = results;
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

    public static GachaEvent MultiResult(string poolId, IReadOnlyList<GachaResult> results)
    {
        return new GachaEvent(
            GachaEventType.RollMultipleResult,
            poolId,
            results: results
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

    public static GachaEvent PoolRegistered(string poolId)
    {
        return new GachaEvent(
            GachaEventType.PoolRegistered,
            poolId
        );
    }

    // =========================
    // HELPERS
    // =========================

    public bool IsSuccess =>
        Type == GachaEventType.RollResult ||
        Type == GachaEventType.RollMultipleResult;

    public bool IsError =>
        Type == GachaEventType.RollError;
}