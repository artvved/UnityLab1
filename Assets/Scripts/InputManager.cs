using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public event Action ClickEvent;

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickEvent?.Invoke();
        }
        
    }
}
