using System.Collections;
using UnityEngine;

public class Ranger : MonoBehaviour
{
    private Player player;
    [SerializeField] private float throwSpeed = 20;
    [SerializeField] private GameObject projectile;


    private void Awake()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
    private void Start()
    {
        StartCoroutine(wait());
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(2);
        PerformAttack();
    }
    public void PerformAttack()
    {
        ThrowAtTarget();

        

    }
    void ThrowAtTarget()
    {
        GameObject p = Instantiate(projectile, gameObject.transform.position + Vector3.up, Quaternion.identity);

        Rigidbody rb = p.GetComponent<Rigidbody>();

        Vector3 targetDirection = player.transform.position - transform.position;

        // Calculate the horizontal distance
        Vector3 horizontalDirection = new Vector3(targetDirection.x, 0, targetDirection.z);
        float horizontalDistance = horizontalDirection.magnitude;

        // Calculate the vertical distance
        float verticalDistance = targetDirection.y;

        // Use a standard ballistic formula to find vertical velocity component
        // This is a simplified approach that assumes a fixed horizontal speed

        float timeToTarget = horizontalDistance / throwSpeed;

        // v_y = y / t - 0.5 * g * t
        float verticalVelocity = (verticalDistance / timeToTarget) - (Physics.gravity.y * timeToTarget / 2f);

        // Calculate the final velocity vector
        Vector3 launchVelocity = horizontalDirection.normalized * throwSpeed + Vector3.up * verticalVelocity;

        // Apply the velocity
        rb.linearVelocity = launchVelocity; // Directly setting velocity works for a one-time launch
    }
}
