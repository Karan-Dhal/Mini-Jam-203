using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] InputActionAsset action;
    private InputAction PauseInputAction;

    [SerializeField] List<GameObject> PauseOpen;
    [SerializeField] AudioMixer mixer;
    [SerializeField] Image _musicDisabled;
    [SerializeField] Image _soundsDisabled;

    public CinemachineInputAxisController camSence;

    public void Start()
    {
        camSence = FindFirstObjectByType<CinemachineInputAxisController>();
        action.FindActionMap("Player").Enable();
        PauseInputAction = InputSystem.actions.FindAction("Pause");
        PauseInputAction.performed += OpenPauseMenu;
        SetCursorLocked();
    }

    private void OpenPauseMenu(InputAction.CallbackContext context)
    {
        OpenPauseMenu();
    }

    private void ClosePauseMenu(InputAction.CallbackContext context)
    {
        
        ClosePauseMenu();
    }

    public void OpenPauseMenu()
    {
        PauseInputAction.performed -= OpenPauseMenu;
        PauseInputAction.performed += ClosePauseMenu;
        SetCursorUnLocked();
        action.FindActionMap("UI").Enable();
        foreach (GameObject go in PauseOpen)
        {
            go.SetActive(true);
        }
    }
    public void ClosePauseMenu()
    {
        PauseInputAction.performed += OpenPauseMenu;
        PauseInputAction.performed -= ClosePauseMenu;
        SetCursorLocked();
        action.FindActionMap("Player").Enable();
        foreach (GameObject go in PauseOpen)
        {
            go.SetActive(false);
        }
    }
    public void SetCursorLocked()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void SetCursorUnLocked()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void SetMusic(float num)
    {
        _musicDisabled.fillAmount = (num - -80) / (0 - -80);
        mixer.SetFloat("MyExposedParam", num);
    }
    public void SetSound(float num)
    {
        _soundsDisabled.fillAmount = (num - -80) / (0 - -80);
        mixer.SetFloat("MyExposedParam 1", num);


    }
    public void SetSensitivity(float num)
    {
        foreach (var c in camSence.Controllers)
        {
            if (c.Name == "Look Orbit X")
            {
                c.Input.Gain = num; // Example: Set gain to 2
            }
            if (c.Name == "Look Orbit Y")
            {
                c.Input.Gain = -num; // Example: Set gain to 2
            }
        }
    }
}
