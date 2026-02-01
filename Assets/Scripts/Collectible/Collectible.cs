using NUnit.Framework;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    void OnTriggerEnter(Collider collision)
    {
        if(collision.TryGetComponent(out Collecter collecter))
        {
            AudioManager.Instance.PlayCollectible();
            collecter.AddScore(1);
            Destroy(gameObject);
        }
    }
}
