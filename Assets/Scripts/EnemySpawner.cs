using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private GameStateManager gameStateManager;
    
    [SerializeField] private DifficultyManager difficultyManager;
    private AnimationCurve spawnDifficultyCurve;
    private AnimationCurve growthDifficultyCurve;
    public PlayerStats PlayerStats { get; set; }

    private Vector2 gamefieldSize = new Vector2(2.5f, 3.2f);

    void Start()
    {
        spawnDifficultyCurve = difficultyManager.SpawnDifficultyCurve;
        growthDifficultyCurve = difficultyManager.GrowthDifficultyCurve;
    }

    public void StartSpawn()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            if (!gameStateManager.IsPlayerAlive)
            {
                break;
            }

            Vector3 pos = new Vector3(
                Random.Range(((-1) * gamefieldSize.x), (gamefieldSize.x)),
                Random.Range(((-1) * gamefieldSize.y), (gamefieldSize.y)),
                0);


            var enemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
            enemy.SecondsToGrow = growthDifficultyCurve.Evaluate(PlayerStats.PointsAmount);
            enemy.OversizeEvent += () =>
            {
                gameStateManager.IsPlayerAlive = false;
                Destroy(enemy.gameObject);
            };
            enemy.GameEndEvent += () =>
            {
                if (!gameStateManager.IsPlayerAlive)
                {
                    Destroy(enemy.gameObject);
                }
            };


            float timeToWait = spawnDifficultyCurve.Evaluate(PlayerStats.PointsAmount);
            
            yield return new WaitForSeconds(timeToWait);
        }
        yield return null;
    }
}