using Gley.EasyIAP;
using UnityEngine;

public class IAPLoading : MonoBehaviour
{
    [SerializeField] private GameObject shield;

    public void SetShield(bool active)
    {
        shield.SetActive(active);
    }
}