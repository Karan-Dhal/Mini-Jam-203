using UnityEngine;

public class BreakingPlatformProxy : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        BreakingPlatform platform = GetComponentInParent<BreakingPlatform>();
        platform.TriggerOnCollisionEnter(collision);
    }
}
