using UnityEngine;

public class HurtBox : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player.gameObject) { player.Damage(1); }
    }
}
