using UnityEngine;

public interface ITimeProvider
{
    float CurrentTime { get; }
}

public class UnityTimeProvider : ITimeProvider
{
    public float CurrentTime => Time.time;
}