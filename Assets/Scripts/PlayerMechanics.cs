using System;
using System.Collections;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMechanics : MonoBehaviour
{

    [SerializeField] InputActionAsset action;
    [SerializeField] private float fastForwardSpeed = 2.0f;
    [SerializeField] private float slowMoSpeed = 1f;
    [SerializeField] private float pauseTime = 5.0f;

    [SerializeField] private GameObject fastF;
    [SerializeField] private GameObject slowM;
    [SerializeField] private GameObject pause;
    [SerializeField] private TMP_Text channel;

    private int currentChannel = 1;

    private InputAction FastForwardInputAction;
    private InputAction SlowMoInputAction;

    private InputAction PauseMechanicInputAction;

    private InputAction ChannelFInputAction;
    private InputAction ChannelBInputAction;

    private bool canChangeChannel = false;

    private Player player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        player = GetComponent<Player>();

        FastForwardInputAction = InputSystem.actions.FindAction("FastForward");
        FastForwardInputAction.performed += FastForward;

        SlowMoInputAction = InputSystem.actions.FindAction("SlowMo");
        SlowMoInputAction.performed += SlowMo;

        PauseMechanicInputAction = InputSystem.actions.FindAction("PauseMechanic");
        PauseMechanicInputAction.performed += Pause;

        ChannelFInputAction = InputSystem.actions.FindAction("ChannelForward");
        ChannelBInputAction = InputSystem.actions.FindAction("ChannelBack");
        ChannelFInputAction.performed += ChangeChannelF;
        ChannelBInputAction.performed += ChangeChannelB;
    }


    private void ChangeChannelF(InputAction.CallbackContext context)
    {
        if (!canChangeChannel) return;
        currentChannel++;
        if (currentChannel == 4) currentChannel = 1;
        foreach (Channel chan in FindObjectsByType<Channel>(FindObjectsSortMode.None)) chan.ChannelChanged(currentChannel);
        channel.text = currentChannel.ToString();
    }

    private void ChangeChannelB(InputAction.CallbackContext context)
    {
        if (!canChangeChannel) return;
        currentChannel--;
        if (currentChannel == 0) currentChannel = 3;
        foreach (Channel chan in FindObjectsByType<Channel>(FindObjectsSortMode.None)) chan.ChannelChanged(currentChannel);
        channel.text = currentChannel.ToString();
    }

    private void Pause(InputAction.CallbackContext context)
    {
        AudioManager.Instance.PlayPause();
        
        foreach (PauseMechanic obj in FindObjectsByType<PauseMechanic>(FindObjectsSortMode.None))
        {
            StartCoroutine(obj.Pause(pauseTime));
            StartCoroutine(Wait(pauseTime));
        }
    }
    public IEnumerator Wait(float _time)
    {
        pause.gameObject.SetActive(true);
        yield return new WaitForSeconds(_time);
        pause.gameObject.SetActive(false);
        
        AudioManager.Instance.PlayUnpause();
    }

    private void FastForward(InputAction.CallbackContext context)
    {
        AudioManager.Instance.PlayFast();

        if (player.speedmult == fastForwardSpeed) { player.speedmult = 1.0f; fastF.gameObject.SetActive(false); }
        else { player.speedmult =  fastForwardSpeed; fastF.gameObject.SetActive(true); }
    }
    private void SlowMo(InputAction.CallbackContext context)
    {
        AudioManager.Instance.PlaySlowmo();

        if (Time.timeScale == slowMoSpeed) { TimeManager.Instance.setWorldSpeed(2.0f); slowM.gameObject.SetActive(false); }
        else { TimeManager.Instance.setWorldSpeed(slowMoSpeed); slowM.gameObject.SetActive(true); }
    }

    public void CanChangeChannel(bool _true)
    {
        channel.gameObject.SetActive(_true);
        canChangeChannel = _true;
    }

}
