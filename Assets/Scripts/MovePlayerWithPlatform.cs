using UnityEngine;

public class MovePlayerWithPlatform : MonoBehaviour
{
    [SerializeField] private MovingPlatform MP;
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().movingPlatform = MP.GetVelocity();
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.parent = null;
        }
    }
}
