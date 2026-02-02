using System;
using System.Collections;
using UnityEngine;

public class CrushingWallsManager : MonoBehaviour
{
    [SerializeField] private GameObject wallLeft;
    [SerializeField] private GameObject wallRight;
    [SerializeField] private float crushInterval = 5f;
    [SerializeField] private float crushDuration = 0.5f;
    [SerializeField] private float amountToCrush = 2f;
    [SerializeField] private bool expandInXDirection = true;
    
    private bool isCrushed = false;
    [HideInInspector] public bool iscrushing = false;
    public bool paused = false;

    void Start()
    {
        StartCoroutine(CrushSequence());
    }

    IEnumerator CrushWalls()
    {
        while (true)
        {
            yield return new WaitForSeconds(crushInterval);
            yield return StartCoroutine(CrushSequence());
        }
    }

    IEnumerator CrushSequence()
    {
        while (true)
        {
            float elapsedTime = 0f;
            while (elapsedTime < crushInterval)
            {
                if (!paused)
                    elapsedTime += Time.deltaTime;
                yield return null;
            }

            if (!isCrushed)
            {
                AudioManager.Instance.PlayCrushingWalls();

                iscrushing = true;
                yield return StartCoroutine(MoveWalls(amountToCrush, -amountToCrush, true));
                iscrushing = false;

                isCrushed = true;
            }
            else
            {
                AudioManager.Instance.PlayCrushingWalls();
            
                yield return StartCoroutine(MoveWalls(-amountToCrush, amountToCrush, false));

                isCrushed = false;
            }
            yield return null;
        }
    }

    IEnumerator MoveWalls(float leftOffset, float rightOffset, bool isExpanding)
    {
        Vector3 leftPosStart = wallLeft.transform.position;
        Vector3 rightPosStart = wallRight.transform.position;
        Vector3 leftPosTarget = expandInXDirection ? GetTargetPositionX(leftPosStart, leftOffset) : GetTargetPositionZ(leftPosStart, leftOffset);
        Vector3 rightPosTarget = expandInXDirection ? GetTargetPositionX(rightPosStart, rightOffset) : GetTargetPositionZ(rightPosStart, rightOffset);

        float elapsedTime = 0f;
        while (elapsedTime < crushDuration)
        {
            if (!paused)
                elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsedTime / crushDuration);

            wallLeft.transform.position = Vector3.Lerp(leftPosStart, leftPosTarget, t);
            wallRight.transform.position = Vector3.Lerp(rightPosStart, rightPosTarget, t);

            yield return null;
        }

        wallLeft.transform.position = leftPosTarget;
        wallRight.transform.position = rightPosTarget;

        isCrushed = !isCrushed;
    }

    Vector3 GetTargetPositionX(Vector3 currentPos, float offset) => new Vector3(currentPos.x + offset, currentPos.y, currentPos.z);
    Vector3 GetTargetPositionZ(Vector3 currentPos, float offset) => new Vector3(currentPos.x, currentPos.y, currentPos.z + offset);
}