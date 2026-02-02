using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMechanics : MonoBehaviour
{

    [SerializeField] InputActionAsset action;
    [SerializeField] private float fastForwardSpeed = 2.0f;
    [SerializeField] private float slowMoSpeed = 1f;
    [SerializeField] private float pauseTime = 5.0f;
    [SerializeField] private float fastFTime = 5.0f;
    [SerializeField] private float slowMoTime = 5.0f;

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

    private bool canFastForward = false;
    private bool canSlowMo = false;
    private bool canPause = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        fastF.SetActive(false);
        slowM.SetActive(false);
        pause.SetActive(false);

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
        if (!canPause) return;
        canPause = false;
        AudioManager.Instance.PlayPause();
        
        foreach (PauseMechanic obj in FindObjectsByType<PauseMechanic>(FindObjectsSortMode.None))
        {
            StartCoroutine(obj.Pause(pauseTime));
        }
        StartCoroutine(Wait(pauseTime));
    }
    public IEnumerator Wait(float _time)
    {
        float _pauseTime = _time;
        Image image = pause.gameObject.GetComponent<Image>();
        while (true)
        {
            if (_pauseTime <= 0) 
            {
                _pauseTime = 0;
                image.fillAmount = _pauseTime;
                break;
            }
            yield return null;
            _pauseTime -= Time.deltaTime;
            image.fillAmount = _pauseTime/pauseTime;
        }
        
        AudioManager.Instance.PlayUnpause();

        while (true)
        {
            if (_pauseTime >= pauseTime)
            {
                _pauseTime = pauseTime;
                image.fillAmount = _pauseTime;
                break;
            }
            yield return null;
            _pauseTime += Time.deltaTime;
            image.fillAmount = _pauseTime / pauseTime;
        }
        canPause = true;
    }

    private void FastForward(InputAction.CallbackContext context)
    {
        if (!canFastForward) return;
        canFastForward = false;
        AudioManager.Instance.PlayFast();
        player.speedmult = fastForwardSpeed;
        StartCoroutine(FastF());
    }
    private void SlowMo(InputAction.CallbackContext context)
    {
        if (!canSlowMo) return;
        canSlowMo = false;
        AudioManager.Instance.PlaySlowmo();
        TimeManager.Instance.setWorldSpeed(slowMoSpeed);
        StartCoroutine(SlowMo());
    }

    public void CanChangeChannel(bool _true)
    {
        channel.gameObject.SetActive(_true);
        canChangeChannel = _true;
    }

    public void CanSlowMo(bool _true)
    {
        slowM.SetActive(_true);
        canSlowMo = _true;
    }
    public void CanFastF(bool _true)
    {
        fastF.SetActive(_true);
        canFastForward = _true;
    }
    public void CanPause(bool _true)
    {
        pause.SetActive(_true);
        canPause = _true;
    }

    IEnumerator SlowMo()
    {
        float _slowTime = slowMoTime;
        Image image = slowM.gameObject.GetComponent<Image>();
        while (true)
        {
            if (_slowTime <= 0)
            {
                _slowTime = 0;
                image.fillAmount = _slowTime;
                break;
            }
            yield return null;
            _slowTime -= Time.deltaTime;
            image.fillAmount = _slowTime / slowMoTime;
        }

        AudioManager.Instance.PlayUnpause();
        TimeManager.Instance.setWorldSpeed(2.0f);

        while (true)
        {
            if (_slowTime >= slowMoTime)
            {
                _slowTime = slowMoTime;
                image.fillAmount = _slowTime;
                break;
            }
            yield return null;
            _slowTime += Time.deltaTime;
            image.fillAmount = _slowTime / slowMoTime;
        }
        canSlowMo = true;
    }

    IEnumerator FastF()
    {
        float _fastTime = fastFTime;
        Image image = fastF.gameObject.GetComponent<Image>();
        while (true)
        {
            if (_fastTime <= 0)
            {
                _fastTime = 0;
                image.fillAmount = _fastTime;
                break;
            }
            yield return null;
            _fastTime -= Time.deltaTime;
            image.fillAmount = _fastTime / fastFTime;
        }

        AudioManager.Instance.PlayUnpause();
        player.speedmult = 1.0f;

        while (true)
        {
            if (_fastTime >= fastFTime)
            {
                _fastTime = fastFTime;
                image.fillAmount = _fastTime;
                break;
            }
            yield return null;
            _fastTime += Time.deltaTime;
            image.fillAmount = _fastTime / fastFTime;
        }
        canFastForward = true;
    }


}
