using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{

    public event Action StartCasualGameEvent;
    public event Action StartLimitedTimeGameEvent;
    public event Action StartColourGameEvent;
    public event Action QuitGameEvent;

    public void OnStartCasualGame()
    {
        StartCasualGameEvent?.Invoke();
    }
    public void OnStartLimitedTimeGame()
    {
        StartLimitedTimeGameEvent?.Invoke();
    }
    public void OnStartColourGame()
    {
        StartColourGameEvent?.Invoke();
    }

    public void OnQuitGame()
    {
        QuitGameEvent?.Invoke();
    }


}
