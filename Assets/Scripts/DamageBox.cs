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
            Debug.LogWarning("In hurtBox");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player.gameObject)
        {
            inHurtbox = false;
            Debug.LogWarning("Out hurtBox");
        }
    }

    public bool IsInHurtbox() {  return inHurtbox; }
}
