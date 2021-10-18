using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerMechanics : MonoBehaviour
{
    private PlayerStats playerStats;

    private Camera mainCamera;

    [SerializeField] private SoundManager soundManager;

    public static PlayerMechanics Instance;

    void Awake()
    {
        playerStats = new PlayerStats();
        mainCamera = Camera.main;
        Instance = this;
    }

    public PlayerStats PlayerStats
    {
        get => playerStats;
        set => playerStats = value;
    }

    private PlayerMechanics()
    {
    }

    void Update()
    {
        if (GameStateManager.Instance.IsPlayerAlive)
        {
            if (Input.GetMouseButtonDown(0))
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
}