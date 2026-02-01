using UnityEngine;

public class ManagerTriggerProxy : MonoBehaviour
{
    [Tooltip("Drag the main GateAndButtonManager here")]
    [SerializeField] private GateAndButtonManager gateAndButtonManager;

    [Tooltip("Assign the specific Button GameObject that moves when this trigger is hit.")]
    [SerializeField] private GameObject linkedButtonObject;

    void Awake()
    {
        if(gateAndButtonManager == null)
        {
            gateAndButtonManager = GetComponentInParent<GateAndButtonManager>();
            
            if(gateAndButtonManager == null)
            {
                Debug.LogError("GateAndButtonManager not found. Please assign it in the Inspector.");
            }
        }

        if (linkedButtonObject == null)
        {
            linkedButtonObject = transform.parent.gameObject;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (gateAndButtonManager != null && linkedButtonObject != null)
            {
                gateAndButtonManager.NotifyButtonPressed(linkedButtonObject);
            }
        }
    }
}