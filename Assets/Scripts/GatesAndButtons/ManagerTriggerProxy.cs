using UnityEngine;

public class ManagerTriggerProxy : MonoBehaviour
{
    [SerializeField] GateAndButtonManager gateAndButtonManager;

    void Awake()
    {
        if(gateAndButtonManager == null)
        {
            if(transform.parent.TryGetComponent(out GateAndButtonManager t))
            {
                gateAndButtonManager = t;
            }
            else
            {
                Debug.LogError("GateAndButtonManager is not assigned in ManagerTriggerProxy AND COULDNT BE FOUND IN PARENT.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gateAndButtonManager.ActivateGateAndButton();
        }
    }
}
