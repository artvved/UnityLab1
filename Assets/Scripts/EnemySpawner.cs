using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    
    
    private PlayerStats playerStats;
    
    
    private AnimationCurve spawnDifficultyCurve;

    private Vector2 gamefieldSize = new Vector2(2.5f,3.2f);
    void Start()
    {
        spawnDifficultyCurve = DifficultyManager.Instance.SpawnDifficultyCurve;
        playerStats = PlayerMechanics.Instance.PlayerStats;
        StartCoroutine(SpawnEnemy());

    }

    private IEnumerator SpawnEnemy()
    {
        
        while (true)
        {
            if (!GameStateManager.Instance.IsPlayerAlive)
            {
                break;
            }
            Vector3 pos = new Vector3(
                Random.Range(((-1) * gamefieldSize.x), (gamefieldSize.x)),
                Random.Range(((-1) * gamefieldSize.y), (gamefieldSize.y)),
                0);
           

            Instantiate(enemyPrefab, pos, Quaternion.identity);
            
            
            float timeToWait = spawnDifficultyCurve.Evaluate(playerStats.PointsAmount); 
            yield return new WaitForSeconds(timeToWait);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
