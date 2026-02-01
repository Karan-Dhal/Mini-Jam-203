using Unity.VisualScripting;
using UnityEngine;

public class KillZ : MonoBehaviour
{
    [SerializeField] private float killZ = -10;
    private Player player;
    bool dead = false;

    private void Awake()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
    void Update()
    {
        if (player.transform.position.y >= killZ) dead = false;
        else if (!dead && player.transform.position.y < killZ)
        {
            dead = true;
            //play Dead
        }
    }
}
