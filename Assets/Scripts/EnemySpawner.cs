using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;

    public EffectManager EffectManager { get; set; }
    public GameStateManager GameStateManager { get; set; }
    public DifficultyManager DifficultyManager { get; set; }

    public SoundManager SoundManager { get; set; }
    private AnimationCurve spawnDifficultyCurve;
    private AnimationCurve growthDifficultyCurve;
    public PlayerStats PlayerStats { get; set; }

    private Vector2 gamefieldSize = new Vector2(2.5f, 3.2f);

    void Start()
    {
        spawnDifficultyCurve = DifficultyManager.SpawnDifficultyCurve;
        growthDifficultyCurve = DifficultyManager.GrowthDifficultyCurve;
    }

    public void StartSpawn()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            if (!GameStateManager.IsPlayerAlive)
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
                GameStateManager.IsPlayerAlive = false;
                EffectManager.PlayEnemyOversizeEffect(enemy.gameObject.transform.position);
                Destroy(enemy.gameObject);
                SoundManager.PlayDeathSound();
            };
            enemy.GameEndEvent += () =>
            {
                if (!GameStateManager.IsPlayerAlive)
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