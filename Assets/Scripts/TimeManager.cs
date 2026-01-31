using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    void Awake()
    {
        Time.timeScale = 2f;



        if (Instance != null && Instance != this)
        {
            // If another instance exists, destroy this duplicate
            Destroy(this.gameObject);
        }
        else
        {
            // If no instance exists, set this as the instance
            Instance = this;
            // Optional: Keep the object alive across scene changes
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void setWorldSpeed(float _speed)
    {
        Time.timeScale = _speed;
    }

}
