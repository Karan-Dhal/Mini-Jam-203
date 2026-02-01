using System.Collections;
using UnityEngine;

public class MoveToPosition : MonoBehaviour
{
    private Vector3 currentTarget;
    private Coroutine activeCoroutine;

    public void StartMoving(Vector3 targetPosition, float duration)
    {
        if (activeCoroutine != null && targetPosition == currentTarget) return;

        currentTarget = targetPosition;
        
        if (activeCoroutine != null) StopCoroutine(activeCoroutine);
        activeCoroutine = StartCoroutine(MoveTo(targetPosition, duration));
    }

    IEnumerator MoveTo(Vector3 target, float duration)
    {
        Vector3 startPosition = transform.position;
        Vector3 finalTarget = new Vector3(startPosition.x, target.y, startPosition.z);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            
            // Use SmoothStep for cleaner movement
            t = Mathf.SmoothStep(0, 1, t); 
            
            transform.position = Vector3.Lerp(startPosition, finalTarget, t);
            yield return null;
        }

        transform.position = finalTarget;
        activeCoroutine = null;
    }
}