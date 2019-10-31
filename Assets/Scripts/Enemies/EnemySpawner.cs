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

    public enum GAMEMODE
    {
        ENEMIESDIE,
        TIMER,
        SURVIVAL
    };

    public GAMEMODE gameMode;

    [Header("Waves Settings")]
    public Wave[] waves;
    [Tooltip("If is active the next wave gonna spawn only " +
             "if all enemies of the current wave die " +
             "(this function ignore timeBetweenWaves)")]
    public bool noTimerMode;
    public bool survivalMode;

    public float timeBetweenWaves = 30f;
    public int maxSurvivalWave = 5;

    private int currentWave = 0;
    private bool isSpawning = true;

    [Header("Spawn Settings")]
    public List<Transform> spawnPoints;
    public List<GameObject> spawnedEnemies;

    [Header("Enemies Settings")]
    public float minEnemySize = 1f;
    public float maxEnemySize = 1.5f;

    [Header("User Settings")]
    public int userSurvivalRecord = 0;

    GameManager gManager;
    UIManager uiManager;

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
        uiManager = UIManager.Get();
        timer = timeBetweenWaves;
        userSurvivalRecord = currentWave;
    }

    private void Update()
    {
        if (gManager.gameStarted)
        {
            if (isSpawning)
            {
                timer -= Time.deltaTime;
                seconds = (int)(timer % 60);
            }

            switch (gameMode)
            {
                case GAMEMODE.ENEMIESDIE:
                    if (currentWave < waves.Length)
                    {
                        EnemiesDieGameMode();
                    }
                    else
                    {
                        allWavesCompleted = true;
                        gManager.gameStarted = false;
                    }
                    break;
                case GAMEMODE.TIMER:
                    if (currentWave < waves.Length)
                    {
                        TimerGameMode();
                    }
                    else
                    {
                        allWavesCompleted = true;
                        gManager.gameStarted = false;
                    }
                    break;
                case GAMEMODE.SURVIVAL:
                    if (gManager.gameStarted)
                    {
                        SurvivalGameMode();
                    }
                    break;
                default:
                    break;
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
        go.transform.GetChild(0).GetComponent<Enemy>().OnDieAction += ScoreManager.Get().AddEnemyKilled;
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

    void TimerGameMode()
    {
        if (seconds <= 0 && isSpawning)
        {
            isSpawning = false;
            uiManager.SetWaveNumber(currentWave + 1);
            StartCoroutine(SpawnWave(waves[currentWave]));
            currentWave++;
        }
    }

    void EnemiesDieGameMode()
    {
        if (spawnedEnemies.Count == 0)
        {
            uiManager.SetWaveNumber(currentWave);
            StartCoroutine(SpawnWave(waves[currentWave]));
            currentWave++;
        }
    }

    void SurvivalGameMode()
    {
        if (seconds <= 0 && isSpawning)
        {
            isSpawning = false;
            
            userSurvivalRecord++;
            uiManager.SetWaveNumber(userSurvivalRecord);

            StartCoroutine(SpawnWave(waves[currentWave]));

            if (currentWave < maxSurvivalWave)
            currentWave++;
        }
    }

    public void ResetTimer()
    {
        timer = timeBetweenWaves;
        isSpawning = true;
    }

    public void ResetSpawner()
    {
        for (int i = 0; i < spawnedEnemies.Count; i++)
        {
            Destroy(spawnedEnemies[i].gameObject);
        }
        spawnedEnemies.Clear();

        ResetTimer();
        userSurvivalRecord = 0;
        currentWave = 0;
    }
    
}
