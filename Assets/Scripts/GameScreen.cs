using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI endGameText;
    [SerializeField] private GameObject colour;
    [SerializeField] private Image colourImage;

    private EnemyColourManager enemyColourManager = new EnemyColourManager();
    
    public event Action PointsChangeEvent;
    public event Action BackToMenuEvent;
    public void OnPointsChange()
    {
        PointsChangeEvent?.Invoke();
    }

    

    public void OnBackToMenu()
    {
        BackToMenuEvent?.Invoke();
    }

    public void SetPoints(int points)
    {
        pointsText.text = "Points : " + points;
    }
    
    
    public void SetTime(int time)
    {
        timeText.text = "Time : " + time;
    }

    public void ShowTime(bool isActive)
    {
        timeText.gameObject.SetActive(isActive);
    }

    public void ShowGoalColour(bool isActive)
    {
        colour.SetActive(isActive);
    }
    public void SetGoalColour(EnemyColour enemyColour)
    {
        var c = enemyColourManager.GetVisualColour(enemyColour);
        colourImage.color = c;
    }

    public void ShowEndGameText(bool isActive)
    {
        endGameText.gameObject.SetActive(isActive);
    }
    
}
