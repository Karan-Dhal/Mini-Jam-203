using System.Collections;
using UnityEngine;

public class MoveToPosition : MonoBehaviour
{
    public void StartMoving(Vector3 offset, float duration)
    {
        StartCoroutine(MoveTo(offset, duration));
    }

    IEnumerator MoveTo(Vector3 target, float duration)
    {
        Vector3 startPosition = transform.position;
        target = new Vector3 (startPosition.x, target.y, startPosition.z);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPosition, target, t);
            yield return null;
        }
    }
}
