using UnityEngine;

public class DamageBox : MonoBehaviour
{
    private Transform player;
    private bool inHurtbox = false;

    private void Awake()
    {
        this.player = GameObject.FindWithTag("Player").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player.gameObject)
        {
            inHurtbox = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player.gameObject)
        {
            inHurtbox = true;
        }
    }

    public bool IsInHurtbox() {  return inHurtbox; }
}
