using UnityEngine;

/// <summary>
/// View chỉ chịu trách nhiệm hiển thị.
/// Không chứa logic business.
/// </summary>
public class AdsView : MonoBehaviour
{
    [SerializeField] private GameObject shield;

    /// <summary>
    /// Hiển thị hoặc ẩn lớp chắn khi show ads
    /// </summary>
    public void SetShieldActive(bool active)
    {
        shield.SetActive(active);
    }
}