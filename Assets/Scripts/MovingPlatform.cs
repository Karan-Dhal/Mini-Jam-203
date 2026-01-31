using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] List<Transform> transforms = new List<Transform>();
    [SerializeField] float speed = 5.0f;
    [SerializeField] float waitAtPos = 1.0f;

    private bool done = false;
    private int index = 0;
    private bool reverse = false;
    private Vector3 previousPosition;

    private void FixedUpdate()
    {
        Move(transforms[index]);
    }

    private void Move(Transform a)
    {
        previousPosition = transform.position;

        transform.position = Vector3.Lerp(transform.position, a.position, speed * Time.deltaTime);
        print(Vector3.Distance(transform.position, a.position));
        if (!done && Vector3.Distance(transform.position, a.position) < 0.1)
        {
            done = true;
            StartCoroutine(wait());
        }
    }

    private void ChangeIndex()
    {
        if (reverse) index--;
        else index++;

        if (index == transforms.Count || index < 0)
        {
            reverse = !reverse;
            if (index < 0) index *= -1;
            else index = transforms.Count - 1;
        }


        done = false;
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(waitAtPos);
        ChangeIndex();
    }

    public Vector3 GetVelocity()
    {
        return(transform.position - previousPosition) / Time.deltaTime;
    }
}
