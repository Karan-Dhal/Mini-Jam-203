using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] private bool detect_in;
    private Enemy enemy;
    private void Awake()
    {
        enemy = transform.parent.GetComponent<Enemy>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (detect_in && other.gameObject.tag == "Player")
        {
            enemy.Detected(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!detect_in && other.gameObject.tag == "Player")
        {
            enemy.Detected(false);
        }
    }

}
