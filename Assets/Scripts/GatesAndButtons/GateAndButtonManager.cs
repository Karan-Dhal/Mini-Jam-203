using UnityEngine;

public class GateAndButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject gate;
    [SerializeField] private Transform gateTopTransform;
    private MoveToPosition gateMover;

    [SerializeField] private GameObject button;
    [SerializeField] private Transform buttonTopTransform;
    private MoveToPosition buttonMover;


    [SerializeField] private float gateDuration = 1f;
    [SerializeField] private float buttonDuration = 0.5f;

    private void Awake()
    {
        gateMover = gate.GetComponent<MoveToPosition>();
        buttonMover = button.GetComponent<MoveToPosition>();

        if(gateMover == null || buttonMover == null)
        {
            Debug.LogError("GateMover or ButtonMover COULD NOT BE FOUND in GateAndButtonManager.");
            return;
        }
        
        if(gateTopTransform == null)
        {
            gateTopTransform = gate.transform.Find("TopPosition");
            if(gateTopTransform == null)
            {
                Debug.LogError("Gate top position is not assigned in GateAndButtonManager AND COULDNT BE FOUND.");
                return;
            }
        }
        
        if(buttonTopTransform == null)
        {
            buttonTopTransform = button.transform.Find("TopPosition");
            if(buttonTopTransform == null)
            {
                Debug.LogError("Button top position is not assigned in GateAndButtonManager AND COULDNT BE FOUND.");
                return;
            }
        }
    }

    public void ActivateGateAndButton()
    {
        Vector3 gateOffset = new Vector3(gateTopTransform.position.x, -gateTopTransform.position.y, gateTopTransform.position.z);
        gateMover.StartMoving(gateOffset, gateDuration);

        Vector3 buttonOffset = new Vector3(buttonTopTransform.position.x, -buttonTopTransform.position.y, buttonTopTransform.position.z);
        buttonMover.StartMoving(buttonTopTransform.position, buttonDuration);
    }
}