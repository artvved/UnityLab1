using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;


    public event Action TimerEvent;

    public void OnTimeChange()
    {
        TimerEvent?.Invoke();
    }

    public int Seconds { get; set; }
    public int CurrentSeconds { get; set; }

    public void StartTimer()
    {
        StartCoroutine(SecondsTimer());
    }

    private IEnumerator SecondsTimer()
    {
        CurrentSeconds = Seconds;

        while (true)
        {
            OnTimeChange();
            CurrentSeconds--;
            if (!gameStateManager.IsPlayerAlive || CurrentSeconds < 0)
            {
                gameStateManager.IsPlayerAlive = false;
                CurrentSeconds = Seconds;
                break;
            }

            yield return new WaitForSeconds(1f);
        }
    }
}