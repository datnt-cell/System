using System;

public enum SettingEventType
{
    SoundChanged,
    MusicChanged,
    VibrationChanged,
    Reset
}

public class SettingEvent
{
    public SettingEventType Type;
    public bool Value;

    public static SettingEvent Sound(bool value)
    {
        return new SettingEvent
        {
            Type = SettingEventType.SoundChanged,
            Value = value
        };
    }

    public static SettingEvent Music(bool value)
    {
        return new SettingEvent
        {
            Type = SettingEventType.MusicChanged,
            Value = value
        };
    }

    public static SettingEvent Vibration(bool value)
    {
        return new SettingEvent
        {
            Type = SettingEventType.VibrationChanged,
            Value = value
        };
    }

    public static SettingEvent Reset()
    {
        return new SettingEvent
        {
            Type = SettingEventType.Reset
        };
    }
}