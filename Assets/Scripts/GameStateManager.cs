using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public bool IsPlayerAlive { get; set; }

   

    void Start()
    {
        
        IsPlayerAlive = false;
    }

    
}
