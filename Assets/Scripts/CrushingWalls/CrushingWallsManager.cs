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

    private Vector3 initialScaleLeft;
    private Vector3 initialScaleRight;
    private bool isCrushed = false;
    [HideInInspector] public bool iscrushing = false;

    void Start()
    {
        initialScaleLeft = wallLeft.transform.localScale;
        initialScaleRight = wallRight.transform.localScale;
        
        StartCoroutine(CrushWalls());
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
        if (!isCrushed)
        {
            iscrushing = true;
            yield return MoveWalls(amountToCrush, -amountToCrush, true);
            iscrushing = false;

            isCrushed = true;
        }
        else
        {
            yield return MoveWalls(-amountToCrush, amountToCrush, false);
            
            isCrushed = false;
        }
    }

    IEnumerator MoveWalls(float leftOffset, float rightOffset, bool isExpanding)
    {
        Vector3 leftPosStart = wallLeft.transform.position;
        Vector3 rightPosStart = wallRight.transform.position;
        Vector3 leftPosTarget = expandInXDirection ? GetTargetPositionX(leftPosStart, leftOffset) : GetTargetPositionZ(leftPosStart, leftOffset);
        Vector3 rightPosTarget = expandInXDirection ? GetTargetPositionX(rightPosStart, rightOffset) : GetTargetPositionZ(rightPosStart, rightOffset);

        Vector3 leftScaleStart = wallLeft.transform.localScale;
        Vector3 rightScaleStart = wallRight.transform.localScale;
        
        Vector3 leftScaleTarget = isExpanding ? GetTargetScale(initialScaleLeft, amountToCrush) : initialScaleLeft;
        Vector3 rightScaleTarget = isExpanding ? GetTargetScale(initialScaleRight, amountToCrush) : initialScaleRight;

        float elapsedTime = 0f;
        while (elapsedTime < crushDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsedTime / crushDuration);

            wallLeft.transform.position = Vector3.Lerp(leftPosStart, leftPosTarget, t);
            wallRight.transform.position = Vector3.Lerp(rightPosStart, rightPosTarget, t);

            wallLeft.transform.localScale = Vector3.Lerp(leftScaleStart, leftScaleTarget, t);
            wallRight.transform.localScale = Vector3.Lerp(rightScaleStart, rightScaleTarget, t);

            yield return null;
        }

        wallLeft.transform.position = leftPosTarget;
        wallRight.transform.position = rightPosTarget;
        wallLeft.transform.localScale = leftScaleTarget;
        wallRight.transform.localScale = rightScaleTarget;
    }

    Vector3 GetTargetScale(Vector3 baseScale, float addedAmount)
    {
        if (expandInXDirection)
            return new Vector3(baseScale.x + addedAmount, baseScale.y, baseScale.z);
        else
            return new Vector3(baseScale.x, baseScale.y, baseScale.z + addedAmount);
    }

    Vector3 GetTargetPositionX(Vector3 currentPos, float offset) => new Vector3(currentPos.x + offset, currentPos.y, currentPos.z);
    Vector3 GetTargetPositionZ(Vector3 currentPos, float offset) => new Vector3(currentPos.x, currentPos.y, currentPos.z + offset);
}