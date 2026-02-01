using UnityEngine;

public class CrushingWall : MonoBehaviour
{
    CrushingWallsManager manager;

    void Start()
    {
        manager = GetComponentInParent<CrushingWallsManager>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (manager.iscrushing)
            {
                collision.gameObject.GetComponent<Player>().Damage(3);
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (manager.iscrushing)
            {
                collision.gameObject.GetComponent<Player>().Damage(3);
            }
        }
    }
}
