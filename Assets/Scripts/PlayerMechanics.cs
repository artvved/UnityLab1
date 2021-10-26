using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerMechanics : MonoBehaviour
{
    private int timeGameSeconds=10;
    private PlayerStats playerStats;

    public PlayerStats PlayerStats
    {
        get => playerStats;
        set => playerStats = value;
    }

    private Camera mainCamera;

    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private InputManager inputManager;

    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private Timer timer;

    [SerializeField] private GameScreen gameScreen;
    [SerializeField] private StartScreen startScreen;


    void Awake()
    {
        playerStats = new PlayerStats();
        mainCamera = Camera.main;
        enemySpawner.PlayerStats = playerStats;

        timer.TimerEvent += () => { gameScreen.SetTime(timer.CurrentSeconds); };

        inputManager.ClickEvent += () =>
        {
            ClickLogic();
            gameScreen.OnPointsChange();
        };
        gameScreen.PointsChangeEvent += () => { gameScreen.SetPoints(playerStats.PointsAmount); };
        gameScreen.BackToMenuEvent += () =>
        {
            gameStateManager.IsPlayerAlive = false;
            playerStats.PointsAmount = 0;
            gameScreen.gameObject.SetActive(false);
            startScreen.gameObject.SetActive(true);
        };
        

        startScreen.StartCasualGameEvent += () =>
        {
            StartGame();
            gameScreen.ShowTime(false);
        };
        startScreen.StartLimitedTimeGameEvent += () =>
        {
            StartGame();
            gameScreen.ShowTime(true);
            timer.Seconds = timeGameSeconds;
            timer.StartTimer();
        };
        startScreen.QuitGameEvent += () =>
        {
            Application.Quit();
        };
    }

    

    private void StartGame()
    {
        gameStateManager.IsPlayerAlive = true;
        gameScreen.gameObject.SetActive(true);
        startScreen.gameObject.SetActive(false);
        enemySpawner.StartSpawn();
    }

    private void ClickLogic()
    {
        if (gameStateManager.IsPlayerAlive)
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                PlayerStats.PointsAmount += 5;
                soundManager.Play();
                Destroy(hit.collider.gameObject);
            }
            else
            {
                if (PlayerStats.PointsAmount != 0)
                {
                    PlayerStats.PointsAmount -= 1;
                }
            }
        }
    }


   
}