using UnityEngine;
using System.Collections.Generic;

public class GateAndButtonManager : MonoBehaviour
{
    // Helper class to organize Button data in Inspector
    [System.Serializable]
    public class ButtonSetup
    {
        public GameObject buttonObject;
        public Transform targetTransform;
        [HideInInspector] public MoveToPosition mover;
        [HideInInspector] public bool isPressed = false;
    }

    [System.Serializable]
    public class GateSetup
    {
        public GameObject gateObject;
        public Transform targetTransform;
        [HideInInspector] public MoveToPosition mover;
    }

    [Header("Configuration")]
    [SerializeField] private List<ButtonSetup> buttons = new List<ButtonSetup>();
    [SerializeField] private List<GateSetup> gates = new List<GateSetup>();

    [Header("Settings")]
    [SerializeField] private float gateDuration = 3f;
    [SerializeField] private float buttonDuration = 1.5f;

    private void Awake()
    {
        InitializeButtons();
        InitializeGates();
    }

    private void InitializeButtons()
    {
        foreach (var btn in buttons)
        {
            if (btn.buttonObject == null) continue;

            btn.mover = btn.buttonObject.GetComponent<MoveToPosition>();
            if (btn.mover == null) Debug.LogError($"MoveToPosition missing on button: {btn.buttonObject.name}");

            if (btn.targetTransform == null)
            {
                btn.targetTransform = btn.buttonObject.transform.Find("TopPosition");
                if (btn.targetTransform == null) Debug.LogError($"TopPosition not found for button: {btn.buttonObject.name}");
            }
        }
    }

    private void InitializeGates()
    {
        foreach (var gt in gates)
        {
            if (gt.gateObject == null) continue;

            gt.mover = gt.gateObject.GetComponent<MoveToPosition>();
            if (gt.mover == null) Debug.LogError($"MoveToPosition missing on gate: {gt.gateObject.name}");

            if (gt.targetTransform == null)
            {
                gt.targetTransform = gt.gateObject.transform.Find("TopPosition");
                if (gt.targetTransform == null) Debug.LogError($"TopPosition not found for gate: {gt.gateObject.name}");
            }
        }
    }

    public void NotifyButtonPressed(GameObject pressedButtonObj)
    {
        ButtonSetup specificButton = buttons.Find(b => b.buttonObject == pressedButtonObj);

        if (specificButton != null)
        {
            if (specificButton.isPressed) return;

            specificButton.isPressed = true;
            MoveItem(specificButton.mover, specificButton.targetTransform, buttonDuration);

            if (AreAllButtonsPressed())
            {
                ActivateAllGates();
            }
        }
        else
        {
            Debug.LogError("The button pressed is not registered in the Manager's list!");
        }
    }

    private bool AreAllButtonsPressed()
    {
        foreach (var btn in buttons)
        {
            if (!btn.isPressed) return false;
        }
        return true;
    }

    private void ActivateAllGates()
    {
        foreach (var gt in gates)
        {
            if (gt.mover != null && gt.targetTransform != null)
            {
                MoveItem(gt.mover, gt.targetTransform, gateDuration);
            }
        }
    }

    private void MoveItem(MoveToPosition mover, Transform target, float duration)
    {
        if (mover == null || target == null) return;

        Vector3 offset = new Vector3(target.position.x, -target.position.y, target.position.z);
        mover.StartMoving(offset, duration);
    }
}