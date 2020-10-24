using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance { get { return instance; } }

    [Header("AI Management")]
    public bool isAINavigating = false;

    public event EventHandler OnStartAINavigations;
    public event EventHandler OnStopAINavigation;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            instance = this;
        }
        else
        {
            instance = this;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
            ToggleAINavigation(!isAINavigating);
    }

    void ToggleAINavigation(bool value)
    {
        switch (value)
        {
            case false:
                OnStopAINavigation?.Invoke(this, EventArgs.Empty);
                break;

            case true:
                OnStartAINavigations?.Invoke(this, EventArgs.Empty);
                break;
        }

        isAINavigating = value;
    }
}
