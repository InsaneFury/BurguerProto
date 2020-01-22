using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    public enum GameMode
    {
        EnemiesDie,
        Timer,
        Survival
    };

    public GameMode gameMode;

    public enum GameDifficulty
    {
        PimientoDelPadron,
        RedHabanero,
        GhostPepper
    };

    public GameDifficulty gameDifficulty;

    public LevelDifficulty[] difficulties;

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
    public int survivalEnemiesLimit = 30;
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
           

            switch (gameMode)
            {
                case GameMode.EnemiesDie:
                    if (gManager.gameStarted)
                    {
                        if(spawnedEnemies.Count == 0)
                        {
                            Timer();
                        }
                        
                        EnemiesDieGameMode();
                    }
                    else
                    {
                        allWavesCompleted = true;
                        gManager.gameStarted = false;
                    }
                    break;
                case GameMode.Timer:
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
                case GameMode.Survival:
                    if (gManager.gameStarted && spawnedEnemies.Count < survivalEnemiesLimit)
                    {
                        Timer();
                        SurvivalGameMode();
                    }
                    break;
                default:
                    break;
            }   
        }
    }

    #region Spawner
    void SpawnRandomEnemy(Wave wave)
    {
        int randEnemy = (int)Random.Range(0f, wave.enemies.Count);
        int randSpawnPoint = (int)Random.Range(0f, spawnPoints.Count);
        float randSize = Random.Range(minEnemySize, maxEnemySize);

        GameObject go = Instantiate(wave.enemies[randEnemy],
            spawnPoints[randSpawnPoint].transform.position,
            wave.enemies[randEnemy].transform.rotation);
        go.transform.localScale = new Vector3(randSize, randSize, randSize);
        go.name = go.name.Replace("(Clone)", "");

        SetEnemyDifficulty(go);
        spawnedEnemies.Add(go);
    }

    IEnumerator SpawnWave(Wave wave)
    {
        for (int i = 0; i < wave.enemyAmount; i++)
        {
            if (gManager.gameStarted)
                SpawnRandomEnemy(wave);
            else
                break;
            yield return new WaitForSecondsRealtime(wave.timeBetweenEnemies);
        }

        yield break;
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
    #endregion

    #region GameModes
    void TimerGameMode()
    {
        if (seconds <= 0 && isSpawning)
        {
            //Audio
            AkSoundEngine.PostEvent("Comienzo_oleada", gameObject);
            isSpawning = false;
            uiManager.SetWaveNumber(currentWave + 1);
            StartCoroutine(SpawnWave(waves[currentWave]));
            currentWave++;
        }
    }

    void EnemiesDieGameMode()
    {
        if ((spawnedEnemies.Count == 0) && isSpawning && (seconds <= 0))
        {
            //Audio
            AkSoundEngine.PostEvent("Comienzo_oleada", gameObject);
            isSpawning = false;

            userSurvivalRecord++;
            uiManager.SetWaveNumber(userSurvivalRecord);
            ScoreManager.Get().SetMaxWave(userSurvivalRecord);
            StartCoroutine(SpawnWave(waves[currentWave]));

            if (currentWave < maxSurvivalWave)
                currentWave++;
        }
    }

    void SurvivalGameMode()
    {
        if (seconds <= 0 && isSpawning)
        {
            //Audio
            AkSoundEngine.PostEvent("Comienzo_oleada", gameObject);
            isSpawning = false;

            userSurvivalRecord++;
            uiManager.SetWaveNumber(userSurvivalRecord);
            ScoreManager.Get().SetMaxWave(userSurvivalRecord);

            StartCoroutine(SpawnWave(waves[currentWave]));

            if (currentWave < maxSurvivalWave)
                currentWave++;
        }
    }

    public void SetGameMode(int mode)
    {
        gameMode = (GameMode)mode;
    }
    #endregion

    #region LevelDifficulty

    void SetEnemyDifficulty(GameObject go)
    {
        if (go.name == "Aji")
        {
            go.GetComponent<NavMeshAgent>().speed = difficulties[(int)gameDifficulty].ajiSpeed;
            go.GetComponent<Enemy>().damage = difficulties[(int)gameDifficulty].ajiDmg;
            go.GetComponent<Enemy>().life = difficulties[(int)gameDifficulty].ajiLife;
        }

        if (go.name == "Tomato")
        {
            Transform child = go.transform.GetChild(0);

            child.GetComponent<NavMeshAgent>().speed = difficulties[(int)gameDifficulty].tomatoSpeed;
            child.GetComponent<Enemy>().damage = difficulties[(int)gameDifficulty].tomatoDmg;
            child.GetComponent<Enemy>().life = difficulties[(int)gameDifficulty].tomatoLife;
        }
    }

    public void SetDifficulty(int levelOfDifficulty)
    {
        gameDifficulty = (GameDifficulty)levelOfDifficulty;
    }
    #endregion

    void Timer()
    {
        if (isSpawning)
        {
            timer -= Time.deltaTime;
            seconds = (int)(timer % 60);
            AkSoundEngine.SetRTPCValue("waves_time", seconds);

            if (seconds == 5)
            {
                uiManager.ActivePopUpAlert("WARNING!", "Next wave is coming");
            }
        }
    }

}
