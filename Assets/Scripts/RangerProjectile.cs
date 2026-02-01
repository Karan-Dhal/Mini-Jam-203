using UnityEngine;

public class RangerProjectile : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float range = 2;
    private Player player;

    private void Awake()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= range) player.Damage(damage);
        //Destroy(gameObject);
    }
}
