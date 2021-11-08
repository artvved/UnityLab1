using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
    private List<GameObject> enemies = new List<GameObject>();
    private Coroutine spawnCouroutine;

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

    public void StartSpawn()
    {
        spawnCouroutine=StartCoroutine(SpawnEnemy());
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

            Vector3 pos = new Vector3(
                Random.Range(((-1) * gamefieldSize.x), (gamefieldSize.x)),
                Random.Range(((-1) * gamefieldSize.y), (gamefieldSize.y)),
                0);


            var enemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
            enemy.SecondsToGrow = growthDifficultyCurve.Evaluate(playerStats.PointsAmount);
            enemy.OversizeEvent += () =>
            {
                gameStateManager.IsPlayerAlive = false;
                effectManager.PlayEnemyOversizeEffect(enemy.gameObject.transform.position);

                soundManager.PlayDeathSound();
                StopCoroutine(SpawnEnemy());
                ClearEnemies();
            };
            DeleteNullsFromEnemies();
            enemies.Add(enemy.gameObject);


            float timeToWait = spawnDifficultyCurve.Evaluate(playerStats.PointsAmount);

            yield return new WaitForSeconds(timeToWait);
        }

        yield return null;
    }

    private void ClearEnemies()
    {
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }

        enemies = new List<GameObject>();
    }

    private void DeleteNullsFromEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
                i--;
            }
        }
    }
}