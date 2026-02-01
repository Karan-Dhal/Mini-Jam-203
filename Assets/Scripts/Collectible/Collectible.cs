using NUnit.Framework;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    void OnTriggerEnter(Collider collision)
    {
        if(collision.TryGetComponent(out Collecter collecter))
        {
            collecter.AddScore(1);
            Destroy(gameObject);
        }
    }
}
