using Unity.VisualScripting;
using UnityEngine;

public class KillY : MonoBehaviour
{
    [SerializeField] private float killZ = -10;
    bool dead = false;
    void Update()
    {
        if (gameObject.transform.position.y >= killZ) dead = false;
        else if (!dead && gameObject.transform.position.y < killZ)
        {
            dead = true;
            gameObject.GetComponent<Player>().Damage(3);
        }
    }
}
