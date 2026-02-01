using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Vector3 checkpointPos;
    private void Awake()
    {
        checkpointPos = gameObject.transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Checkpoint")
        {
            checkpointPos = other.transform.position;
            gameObject.GetComponent<Player>().ResetHealth();
        }
    }

    public void ReturnToCheckpoint()
    {
        Debug.LogError("Ded");
        gameObject.transform.position = checkpointPos;
        
    }
}
