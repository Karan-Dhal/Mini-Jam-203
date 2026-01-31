using System.Collections;
using UnityEngine;

public class PauseMechanic : MonoBehaviour
{
    
    private Rigidbody rb;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    public IEnumerator Pause(float _time)
    {
        Vector3 velocity = rb.linearVelocity;
        rb.isKinematic = true;
        anim.speed = 0f;

        yield return new WaitForSeconds(_time);

        rb.isKinematic = false;
        anim.speed = 1f;
        rb.linearVelocity = velocity;
    }
}
