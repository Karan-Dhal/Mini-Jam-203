using UnityEngine;

public class Melee : MonoBehaviour
{
    [SerializeField] private DamageBox damagebox;
    [SerializeField] private int damage = 1;
    private Player player;

    private void Awake()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
    public void PerformAttack()
    {
        if (damagebox.IsInHurtbox())
        {
            //player DO DAMAGE set this up through animation
            player.Damage(damage);
        }
    }
}
