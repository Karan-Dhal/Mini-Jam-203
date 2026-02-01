using UnityEngine;

public class ManagerTriggerProxy : MonoBehaviour
{
    [SerializeField] private GateAndButtonManager gateAndButtonManager;
    [SerializeField] private GameObject linkedButtonObject;

    void Awake()
    {
        if(gateAndButtonManager == null)
        {
            gateAndButtonManager = GetComponentInParent<GateAndButtonManager>();
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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gateAndButtonManager != null && linkedButtonObject != null)
            {
                gateAndButtonManager.NotifyButtonReleased(linkedButtonObject);
            }
        }
    }
}