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
                AkSoundEngine.SetRTPCValue("waves_time", seconds);

                if(seconds == 5)
                {
                    uiManager.ActivePopUpAlert("WARNING!", "Next wave is coming");
                }
            }

            switch (gameMode)
            {
                case GameMode.EnemiesDie:
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

        switch (gameDifficulty)
        {
            case GameDifficulty.PimientoDelPadron:
                SetPimientoDelPadron(go);
                break;
            case GameDifficulty.RedHabanero:
                SetRedHabanero(go);
                break;
            case GameDifficulty.GhostPepper:
                SetGhostPepper(go);
                break;
            default:
                break;
        }


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
        if (spawnedEnemies.Count == 0)
        {
            //Audio
            AkSoundEngine.PostEvent("Comienzo_oleada", gameObject);
            uiManager.SetWaveNumber(currentWave);
            StartCoroutine(SpawnWave(waves[currentWave]));
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

            StartCoroutine(SpawnWave(waves[currentWave]));

            if (currentWave < maxSurvivalWave)
                currentWave++;
        }
    }
    #endregion

    #region LevelDifficulty
    void SetPimientoDelPadron(GameObject go)
    {
        if (go.name == "Aji")
        {
            go.GetComponent<NavMeshAgent>().speed = difficulties[0].ajiSpeed;
            go.GetComponent<Enemy>().damage = difficulties[0].ajiDmg;
            go.GetComponent<Enemy>().life = difficulties[0].ajiLife;
        }

        if (go.name == "Tomato")
        {
            Transform child = go.transform.GetChild(0);

            child.GetComponent<NavMeshAgent>().speed = difficulties[0].tomatoSpeed;
            child.GetComponent<Enemy>().damage = difficulties[0].tomatoDmg;
            child.GetComponent<Enemy>().life = difficulties[0].tomatoLife;
        }
    }

    void SetRedHabanero(GameObject go)
    {
        if (go.name == "Aji")
        {
            Transform child = go.transform.GetChild(0);

            go.GetComponent<NavMeshAgent>().speed = difficulties[1].ajiSpeed;
            go.GetComponent<Enemy>().damage = difficulties[1].ajiDmg;
            go.GetComponent<Enemy>().life = difficulties[1].ajiLife;
        }

        if (go.name == "Tomato")
        {
            Transform child = go.transform.GetChild(0);

            child.GetComponent<NavMeshAgent>().speed = difficulties[1].tomatoSpeed;
            child.GetComponent<Enemy>().damage = difficulties[1].tomatoDmg;
            child.GetComponent<Enemy>().life = difficulties[1].tomatoLife;
        }
    }

    void SetGhostPepper(GameObject go)
    {
        if (go.name == "Aji")
        {
            go.GetComponent<NavMeshAgent>().speed = difficulties[2].ajiSpeed;
            go.GetComponent<Enemy>().damage = difficulties[2].ajiDmg;
            go.GetComponent<Enemy>().life = difficulties[2].ajiLife;
        }

        if (go.name == "Tomato")
        {
            Transform child = go.transform.GetChild(0);

            child.GetComponent<NavMeshAgent>().speed = difficulties[2].tomatoSpeed;
            child.GetComponent<Enemy>().damage = difficulties[2].tomatoDmg;
            child.GetComponent<Enemy>().life = difficulties[2].tomatoLife;
        }
    }

    public void SetDifficulty(int levelOfDifficulty)
    {
        gameDifficulty = (GameDifficulty)levelOfDifficulty;
    }
    #endregion

}
