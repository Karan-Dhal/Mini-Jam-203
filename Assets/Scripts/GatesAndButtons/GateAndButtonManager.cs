using UnityEngine;
using System.Collections.Generic;

public class GateAndButtonManager : MonoBehaviour
{
    public enum ButtonMode
    {
        Switch,
        Button
    }

    [System.Serializable]
    public class ButtonSetup
    {
        public GameObject buttonObject;
        public Vector3 targetPosition;
        public ButtonMode interactMode;
        [HideInInspector] public MoveToPosition mover;
        [HideInInspector] public bool isPressed = false;
        [HideInInspector] public Vector3 initialPosition;
    }

    [System.Serializable]
    public class GateSetup
    {
        public GameObject gateObject;
        public Vector3 targetPosition;
        [HideInInspector] public MoveToPosition mover;
        [HideInInspector] public Vector3 initialPosition;
    }

    [SerializeField] private List<ButtonSetup> buttons = new List<ButtonSetup>();
    [SerializeField] private List<GateSetup> gates = new List<GateSetup>();
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
            btn.initialPosition = btn.buttonObject.transform.position;

            if (btn.targetPosition == null)
            {
                btn.targetPosition = btn.buttonObject.transform.Find("ClosedPosition").position;
            }
        }
    }

    private void InitializeGates()
    {
        foreach (var gt in gates)
        {
            if (gt.gateObject == null) continue;

            gt.mover = gt.gateObject.GetComponent<MoveToPosition>();
            gt.initialPosition = gt.gateObject.transform.position;

            if (gt.targetPosition == null)
            {
                gt.targetPosition = gt.gateObject.transform.Find("ClosedPosition").position;
            }
        }
    }

    public void NotifyButtonPressed(GameObject pressedButtonObj)
    {
        ButtonSetup specificButton = buttons.Find(b => b.buttonObject == pressedButtonObj);

        if (specificButton != null)
        {
            if (specificButton.isPressed) return;

            if (specificButton.interactMode == ButtonMode.Switch && specificButton.isPressed) return;

            specificButton.isPressed = true;
            MoveItem(specificButton.mover, specificButton.targetPosition, buttonDuration);
            CheckGateState();
        }
    }

    public void NotifyButtonReleased(GameObject releasedButtonObj)
    {
        ButtonSetup specificButton = buttons.Find(b => b.buttonObject == releasedButtonObj);

        if (specificButton != null)
        {
            if (!specificButton.isPressed) return;

            if (specificButton.interactMode == ButtonMode.Button)
            {
                specificButton.isPressed = false;
                MoveItem(specificButton.mover, specificButton.initialPosition, buttonDuration);
                CheckGateState();
            }
        }
    }

    private void CheckGateState()
    {
        if (AreAllButtonsPressed())
        {
            MoveGates(true);
        }
        else
        {
            MoveGates(false);
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

    private void MoveGates(bool open)
    {
        foreach (var gt in gates)
        {
            if (gt.mover != null)
            {
                Vector3 target = open ? gt.targetPosition : gt.initialPosition;
                MoveItem(gt.mover, target, gateDuration);
            }
        }
    }

    private void MoveItem(MoveToPosition mover, Vector3 targetPos, float duration)
    {
        if (mover == null) return;
        mover.StartMoving(targetPos, duration);
    }
}