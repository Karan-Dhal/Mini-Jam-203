using UnityEngine;

public class BreakingPlatform : MonoBehaviour
{
    [SerializeField] private float breakDelay = 0.5f;
    [SerializeField] private float respawnDelay = 3f;
    [SerializeField] private bool willRespawn = true;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Invoke(nameof(BreakPlatform), breakDelay);
        }
    }

    public void TriggerOnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Invoke(nameof(BreakPlatform), breakDelay);
        }
    }

    private void BreakPlatform()
    {
        disablePlatform();

        if (willRespawn)
            Invoke(nameof(RespawnPlatform), respawnDelay);
    }

    private void RespawnPlatform()
    {
        enablePlatform();
    }

    private void disablePlatform()
    {
        gameObject.SetActive(false);
    }

    private void enablePlatform()
    {
        gameObject.SetActive(true);
    }
}