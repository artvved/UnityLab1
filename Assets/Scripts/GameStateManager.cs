using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public bool IsPlayerAlive { get; set; }

    public static GameStateManager Instance;

    private GameStateManager()
    {
    }

    void Start()
    {
        Instance = this;
        IsPlayerAlive = true;
    }

    
}
