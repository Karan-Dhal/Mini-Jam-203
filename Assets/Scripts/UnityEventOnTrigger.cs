using UnityEngine;
using UnityEngine.Events;

public class UnityEventOnTrigger : MonoBehaviour
{
    private Transform player;
    [SerializeField] private UnityEvent Event;

    private void Awake()
    {
        this.player = GameObject.FindWithTag("Player").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player.gameObject)
        {
            Event.Invoke();
        }
    }
}
