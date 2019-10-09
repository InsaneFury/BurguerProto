using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonobehaviourSingleton<EnemySpawner>
{
    [System.Serializable]
    public class Wave
    {
        public string name;
        public int enemyAmount;
        public List<GameObject> enemies;
        public float timeBetweenEnemies;
    }

    [Header("Waves Settings")]
    public Wave[] waves;
    [Tooltip("If is active the next wave gonna spawn only " +
        "if all enemies of the current wave die " +
        "(this function ignore timeBetweenWaves)")]
    public bool nextWaveIfEmpty;
    public float timeBetweenWaves = 30f;
    int currentWave = 0;

    [Header("Spawn Settings")]
    public List<Transform> spawnPoints;
    public List<GameObject> spawnedEnemies;

    [Header("Enemies Settings")]
    public float minEnemySize = 1f;
    public float maxEnemySize = 1.5f;

    GameManager gManager;
    public float timer = 0;
    [HideInInspector]
    public int seconds = 0;
    [HideInInspector]
    public bool allWavesCompleted = false;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        spawnedEnemies = new List<GameObject>();
        gManager = GameManager.Get();
        timer = timeBetweenWaves;
    }

    private void Update()
    {
        if (gManager.gameStarted)
        {
            timer -= Time.deltaTime;
            seconds = (int)(timer % 60);

            if(currentWave < waves.Length)
            {
                WavesSpawner();
            }
            else
            {
                allWavesCompleted = true;
                gManager.gameStarted = false;
            }
        }
    }

    void SpawnRandomEnemy(Wave wave)
    {
        int randEnemy = (int)Random.Range(0f, wave.enemies.Count);
        int randSpawnPoint = (int)Random.Range(0f, spawnPoints.Count);
        float randSize = Random.Range(minEnemySize, maxEnemySize);

        GameObject go = Instantiate(wave.enemies[randEnemy], 
            spawnPoints[randSpawnPoint].transform.position,
            wave.enemies[randEnemy].transform.rotation);
        go.transform.localScale = new Vector3(randSize,randSize,randSize);

        spawnedEnemies.Add(go);
    }

    IEnumerator SpawnWave(Wave wave)
    {
        for(int i = 0; i < wave.enemyAmount; i++)
        {
            SpawnRandomEnemy(wave);
            yield return new WaitForSecondsRealtime(wave.timeBetweenEnemies);
        }

        yield break;
    }

    void WavesSpawner()
    {
        if (nextWaveIfEmpty)
        {
            if(spawnedEnemies.Count == 0)
            {
                StartCoroutine(SpawnWave(waves[currentWave]));
                currentWave++;
            }  
        }
        else
        {
            if (seconds <= 0)
            {
                StartCoroutine(SpawnWave(waves[currentWave]));
                timer = timeBetweenWaves;
                currentWave++;
            }
        } 
    }
    
}
