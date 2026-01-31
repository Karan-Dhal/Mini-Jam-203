using UnityEngine;

public class Channel : MonoBehaviour
{
    [SerializeField, Range(1,3)] private int channel = 1;
    void Awake()
    {
        ChannelChanged(1);
    }

    public void ChannelChanged(int _channel)
    {
        if (channel == _channel) gameObject.layer = LayerMask.NameToLayer("Default");
        else gameObject.layer = LayerMask.NameToLayer("NotInteractable");
    }
}
