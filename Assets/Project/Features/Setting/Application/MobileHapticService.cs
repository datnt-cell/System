using Solo.MOST_IN_ONE;
using UnityEngine;

/// <summary>
/// Triển khai rung bằng Mobile Haptic Feedback
/// </summary>
public class MobileHapticService : IHapticService
{
    public void Light()
    {
        MOST_HapticFeedback.Generate(MOST_HapticFeedback.HapticTypes.LightImpact);
    }
}