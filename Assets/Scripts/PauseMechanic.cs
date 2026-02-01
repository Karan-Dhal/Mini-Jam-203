using System.Collections;
using UnityEngine;

public class PauseMechanic : MonoBehaviour
{
    
    private Rigidbody rb = null;
    [SerializeField] private CrushingWallsManager Walls = null;
    [SerializeField] private MoveToPosition move = null;

    private void Awake()
    {
        if (Walls != null)
        {

        }
        if (move != null)
        {

        }
        if (rb != null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }
    public IEnumerator Pause(float _time)
    {
        Vector3 velocity = Vector3.zero;
        if (Walls != null)
        {
            Walls.paused = true;
        }
        if (move != null)
        {
            move.paused = true;
        }
        if (rb != null)
        {
            velocity = rb.linearVelocity;
            rb.isKinematic = true;
        }

        

        yield return new WaitForSeconds(_time);

        if (Walls != null)
        {
            Walls.paused = false;
        }
        if (move != null)
        {
            move.paused = false;
        }
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.linearVelocity = velocity;
        }
        
    }
}
