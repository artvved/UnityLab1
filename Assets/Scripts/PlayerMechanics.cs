using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerMechanics : MonoBehaviour
{
    private int timeGameSeconds = 10;
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
    [SerializeField] private EffectManager effectManager;
    [SerializeField] private DifficultyManager difficultyManager;

    //UI
    [SerializeField] private GameScreen gameScreen;
    [SerializeField] private StartScreen startScreen;


    void Awake()
    {
        playerStats = new PlayerStats();
        mainCamera = Camera.main;
        enemySpawner.Construct(effectManager, gameStateManager, difficultyManager, soundManager, playerStats);

        timer.TimerEvent += () => { gameScreen.SetTime(timer.CurrentSeconds); };

        inputManager.ClickEvent += () =>
        {
            ClickLogic();
            gameScreen.OnPointsChange();
        };
        gameScreen.PointsChangeEvent += () => { gameScreen.SetPoints(playerStats.PointsAmount); };
        gameScreen.BackToMenuEvent += () =>
        {
            enemySpawner.StopAndClear();
            enemySpawner.CurrentColor = EnemyColour.DEFAULT;
            gameStateManager.IsPlayerAlive = false;
            playerStats.PointsAmount = 0;
            gameScreen.gameObject.SetActive(false);
            startScreen.gameObject.SetActive(true);
        };
        startScreen.StartColourGameEvent += () =>
        {
            StartGame();
            gameScreen.ShowTime(false);
            gameScreen.ShowGoalColour(true);
            //gameScreen.SetGoalColour(EnemyColour.DEFAULT);
            enemySpawner.StartColourSpawn();
        };


        startScreen.StartCasualGameEvent += () =>
        {
            StartGame();
            enemySpawner.StartSpawn();
            gameScreen.ShowTime(false);
        };
        startScreen.StartLimitedTimeGameEvent += () =>
        {
            StartGame();
            enemySpawner.StartSpawn();
            gameScreen.ShowTime(true);
            timer.Seconds = timeGameSeconds;
            timer.StartTimer();
        };
        startScreen.QuitGameEvent += () => { Application.Quit(); };

        enemySpawner.SpawnIterationEvent += () =>
        {
            gameScreen.SetGoalColour(enemySpawner.CurrentColor);
        };
    }


    private void StartGame()
    {
        gameStateManager.IsPlayerAlive = true;
        gameScreen.gameObject.SetActive(true);
        startScreen.gameObject.SetActive(false);
    }

    private void ClickLogic()
    {
        if (gameStateManager.IsPlayerAlive)
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                var enemy = hit.collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    if (enemy.EnemyColour != EnemyColour.DEFAULT)
                    {  //colour game
                        if (enemy.EnemyColour == enemySpawner.CurrentColor)
                        {
                            SuccessHit(hit);
                            enemySpawner.AnimatedClearEnemies();
                        }
                        else
                        {
                            gameStateManager.IsPlayerAlive = false;
                            effectManager.PlayEnemyOversizeEffect(enemy.gameObject.transform.position);
                            soundManager.PlayDeathSound();
                            enemySpawner.StopAndClear();
                        }


                    }
                    else
                    {   //casual game
                        SuccessHit(hit);
                        enemy.PlayDeathAnimationAndDie();
                    }
                }
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

    private void SuccessHit(RaycastHit2D hit)
    {
        PlayerStats.PointsAmount += 5;
        soundManager.Play();
        effectManager.PlayEnemyClickEffect(hit.transform.position);
    }
}