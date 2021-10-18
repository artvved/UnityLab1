using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoseTextManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
        if (!GameStateManager.Instance.IsPlayerAlive)
        {
            text.enabled = true;
        }
    }
}
