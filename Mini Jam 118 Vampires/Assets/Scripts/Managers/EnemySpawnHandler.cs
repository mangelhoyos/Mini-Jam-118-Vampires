using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnHandler : MonoBehaviour
{
    public static EnemySpawnHandler Instance {private set; get;}

    [SerializeField]
    private Transform[] normalEnemiesSpawnPoints;
    [SerializeField]
    private Transform[] staticEnemiesSpawnPoints;

    [SerializeField]
    private GameObject staticEnemyPrefab;
    [SerializeField]
    private GameObject normalEnemyPrefab;

    private int enemiesLimit = 8;
    private int enemiesMax = 34;

    public int actualNormalEnemies = 0;
    public int actualStaticEnemies = 0;

    void Awake()
    {
        Instance = this;
    }

    public void StartSpawning()
    {
        StartCoroutine(nameof(SpawnerCoroutine));
    }

    IEnumerator SpawnerCoroutine()
    {
        while(true)
        {
            if((actualNormalEnemies + actualStaticEnemies) <= enemiesLimit)
            {
                if(actualStaticEnemies < 4)
                {
                    SpawnStaticEnemy();
                }
                else
                {
                    SpawnNormalEnemy();
                }
            }
            else
            {
                break;
            }
        }

        enemiesLimit += 2;
        enemiesLimit = Mathf.Clamp(enemiesLimit, 0, enemiesMax);
        yield return new WaitForSeconds(60f);
        StartCoroutine(nameof(SpawnerCoroutine));
    }

    void SpawnNormalEnemy()
    {
        int ran = Random.Range(0, normalEnemiesSpawnPoints.Length);

        Instantiate(normalEnemyPrefab, normalEnemiesSpawnPoints[ran].position, Quaternion.identity);
        actualNormalEnemies++;
    }

    void SpawnStaticEnemy()
    {
        int ran = Random.Range(0, staticEnemiesSpawnPoints.Length);

        Instantiate(staticEnemyPrefab, staticEnemiesSpawnPoints[ran].position, Quaternion.identity);
        actualStaticEnemies++;
    }
    
}
