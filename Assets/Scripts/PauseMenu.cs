using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] InputActionAsset action;
    private InputAction PauseInputAction;

    [SerializeField] List<GameObject> PauseOpen;
    [SerializeField] AudioMixer mixer;
    [SerializeField] GameObject _music;
    [SerializeField] GameObject _sounds;


    public void Start()
    {
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
    }
    public void SetCursorUnLocked()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void SetMusic(bool _true)
    {
        _music.SetActive(!_true);
        if (_true) mixer.SetFloat("MyExposedParam", 0);
        else mixer.SetFloat("MyExposedParam", -80);
    }
    public void SetSound(bool _true)
    {
        _sounds.SetActive(!_true);
        if (_true) mixer.SetFloat("MyExposedParam 1", 0);
        else mixer.SetFloat("MyExposedParam 1", -80);

    }
}
