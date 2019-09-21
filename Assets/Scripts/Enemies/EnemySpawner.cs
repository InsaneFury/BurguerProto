using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonobehaviourSingleton<EnemySpawner>
{
    [Header("General Settings")]
    public float timeToSpawn = 5f;
    public List<GameObject> enemies;
    public List<GameObject> bosses;
    public List<Transform> spawnPoints;
    public List<GameObject> spawnedEnemies;

    [Header("Enemies Settings")]
    public float minEnemySize = 0.5f;
    public float maxEnemySize = 1.5f;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        spawnedEnemies = new List<GameObject>();
        InvokeRepeating("SpawnRandomEnemy", 0f , timeToSpawn);
    }

    void SpawnRandomEnemy()
    {
        int randEnemy = (int)Random.Range(0f, enemies.Count);
        int randSpawnPoint = (int)Random.Range(0f, spawnPoints.Count);
        float randSize = Random.Range(minEnemySize, maxEnemySize);

        GameObject go = Instantiate(enemies[randEnemy], 
            spawnPoints[randSpawnPoint].transform.position, 
            enemies[randEnemy].transform.rotation);
        go.transform.localScale = new Vector3(randSize,randSize,randSize);

        spawnedEnemies.Add(go);
    }
}
