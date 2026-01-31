using UnityEngine;

public class ChannelArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        print("Entered");
        if (collision.gameObject.tag == "Player")
        {
            print("Player Entered");
            collision.gameObject.GetComponent<PlayerMechanics>().CanChangeChannel(true);
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        print("Exit");
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMechanics>().CanChangeChannel(false);
        }
    }
}
