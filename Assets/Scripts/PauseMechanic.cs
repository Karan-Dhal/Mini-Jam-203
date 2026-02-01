using System.Collections;
using UnityEngine;

public class PauseMechanic : MonoBehaviour
{
    private Enemy enemy;
    private Rigidbody rb;
    private Vector3 velocity;
    private Animator anim;

    private void Awake()
    {
        if (TryGetComponent<Enemy>(out Enemy _enemy)) enemy = _enemy;
        else enemy = null;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    public IEnumerator Pause(float _time)
    {
        if (enemy != null)
        {
            enemy.Paused();
        }
        else
        {
            velocity = rb.linearVelocity;
            rb.isKinematic = true;
        }

        anim.speed = 0f;

        yield return new WaitForSeconds(_time);

        if (enemy != null)
        {
            enemy.Play();
        }
        else
        {
            rb.isKinematic = false;
            rb.linearVelocity = velocity;
        }
        anim.speed = 1f;
    }
}
