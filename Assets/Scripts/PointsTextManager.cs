using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointsTextManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private PlayerMechanics playerMechanics;

    private PlayerStats playerStats;
    void Start()
    {
       playerStats= playerMechanics.PlayerStats;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Points : " + playerStats.PointsAmount;
    }
}
