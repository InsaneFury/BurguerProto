using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonobehaviourSingleton<EnemySpawner>
{
    [Header("Settings")]
    public float timeToSpawn = 5f;
    public List<GameObject> enemies;
    public List<GameObject> bosses;
    public List<Transform> spawnPoints;
    public List<GameObject> spawnedEnemies;

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
        float randSize = Random.Range(0.5f, 1f);

        GameObject go = Instantiate(enemies[randEnemy], 
            spawnPoints[randSpawnPoint].transform.position, 
            enemies[randEnemy].transform.rotation);
        Debug.Log(randSize);
        go.transform.localScale = new Vector3(randSize,randSize,randSize);

        spawnedEnemies.Add(go);
    }
}
