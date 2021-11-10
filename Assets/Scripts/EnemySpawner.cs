using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;

    private EffectManager effectManager;
    private GameStateManager gameStateManager;
    private DifficultyManager difficultyManager;
    private SoundManager soundManager;

    private AnimationCurve spawnDifficultyCurve;
    private AnimationCurve growthDifficultyCurve;

    private PlayerStats playerStats;
    private Vector2 gamefieldSize = new Vector2(2.5f, 3.2f);
    private List<GameObject> enemiesGO = new List<GameObject>();
    private Coroutine spawnCouroutine;
    private List<EnemyColour> colors = new List<EnemyColour>() {EnemyColour.RED, EnemyColour.GREEN, EnemyColour.BLUE};
    public EnemyColour CurrentColor;

    void Start()
    {
        spawnDifficultyCurve = difficultyManager.SpawnDifficultyCurve;
        growthDifficultyCurve = difficultyManager.GrowthDifficultyCurve;
    }

    public void Construct(EffectManager effectManager, GameStateManager gameStateManager,
        DifficultyManager difficultyManager, SoundManager soundManager, PlayerStats playerStats)
    {
        this.effectManager = effectManager;
        this.gameStateManager = gameStateManager;
        this.difficultyManager = difficultyManager;
        this.soundManager = soundManager;
        this.playerStats = playerStats;
    }

    public event Action SpawnIterationEvent;

    public void OnSpawnIteration()
    {
        SpawnIterationEvent?.Invoke();
    }

    public void StartSpawn()
    {
        spawnCouroutine = StartCoroutine(SpawnEnemy());
    }

    public void StartColourSpawn()
    {
        spawnCouroutine = StartCoroutine(SpawnColourEnemy());
    }

    public void StopSpawn()
    {
        StopCoroutine(spawnCouroutine);
    }

    public void StopAndClear()
    {
        ClearEnemies();
        StopSpawn();
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            if (!gameStateManager.IsPlayerAlive)
            {
                break;
            }

            Vector3 pos = GetRandomPosition();
            var enemy = ConstructEnemy(pos, EnemyColour.DEFAULT);
            DeleteNullsFromEnemies();
            enemiesGO.Add(enemy.gameObject);

            OnSpawnIteration();
            float timeToWait = spawnDifficultyCurve.Evaluate(playerStats.PointsAmount);
            yield return new WaitForSeconds(timeToWait);
        }

        yield return null;
    }

    private IEnumerator SpawnColourEnemy()
    {
        while (true)
        {
            if (!gameStateManager.IsPlayerAlive)
            {
                break;
            }

            DeleteNullsFromEnemies();

            if (enemiesGO.Count == 0)
            {
                CurrentColor = colors[Random.Range(0, colors.Count - 1)];

                foreach (var color in colors)
                {
                    Vector3 pos = GetRandomPosition();
                    var enemy = ConstructEnemy(pos, color);
                    enemiesGO.Add(enemy.gameObject);
                }
            }

            OnSpawnIteration();
            float timeToWait = spawnDifficultyCurve.Evaluate(playerStats.PointsAmount);
            yield return new WaitForSeconds(timeToWait);
        }

        yield return null;
    }

    private Enemy ConstructEnemy(Vector3 pos, EnemyColour enemyColour)
    {
        var enemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
        enemy.SecondsToGrow = growthDifficultyCurve.Evaluate(playerStats.PointsAmount);
        enemy.SetEnemyColour(enemyColour);
        enemy.OversizeEvent += () =>
        {
            gameStateManager.IsPlayerAlive = false;
            effectManager.PlayEnemyOversizeEffect(enemy.gameObject.transform.position);

            soundManager.PlayDeathSound();
            StopAndClear();
        };
        return enemy;
    }

    private void ClearEnemies()
    {
        foreach (var enemy in enemiesGO)
        {
            Destroy(enemy);
        }

        enemiesGO = new List<GameObject>();
    }

    public void AnimatedClearEnemies()
    {
        foreach (var enemy in enemiesGO)
        {
            enemy.GetComponent<Enemy>().PlayDeathAnimationAndDie();
        }

        enemiesGO = new List<GameObject>();
    }

    private void DeleteNullsFromEnemies()
    {
        for (int i = 0; i < enemiesGO.Count; i++)
        {
            if (enemiesGO[i] == null)
            {
                enemiesGO.RemoveAt(i);
                i--;
            }
        }
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector3(
            Random.Range(((-1) * gamefieldSize.x), (gamefieldSize.x)),
            Random.Range(((-1) * gamefieldSize.y), (gamefieldSize.y)),
            0);
    }
}