﻿using System.Collections;
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
        Classic,
        Madness,
        Nightmare
    };

    public GameModeSetting customGameMode;
    public GameDifficulty gameDifficulty;

    public LevelDifficulty[] difficulties;

    [Header("Waves Settings")]
    public Wave[] lvl1Waves;
    public Wave[] lvl2Waves;
    [Tooltip("If is active the next wave gonna spawn only " +
             "if all enemies of the current wave die " +
             "(this function ignore timeBetweenWaves)")]
    public bool noTimerMode;
    public bool survivalMode;

    public float timeBetweenWaves = 30f;
    public int maxSurvivalWave = 10;

    private int currentWave = 0;
    private bool isSpawning = true;

    [Header("Spawn Settings")]
    public List<Transform> lvl1SpawnPoints;
    public List<Transform> lvl2SpawnPoints;
    public List<GameObject> spawnedEnemies;
    public Transform BossSpawnPoint;

    [Header("Enemies Settings")]
    public int survivalEnemiesLimit = 30;
    public float minEnemySize = 1f;
    public float maxEnemySize = 1.5f;

    [Header("User Settings")]
    public int userSurvivalRecord = 0;

    GameManager gManager;
    UIManager uiManager;
    ScenesManagerHandler sceneHandler;

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
        sceneHandler = ScenesManagerHandler.Get();
        spawnedEnemies = new List<GameObject>();
        gManager = GameManager.Get();
        uiManager = UIManager.Get();
        timer = timeBetweenWaves;
        userSurvivalRecord = currentWave;
        if (customGameMode.map1)
        {
            SetDifficulty((int)GameDifficulty.Classic);
            SetGameMode((int)GameMode.EnemiesDie);
        }
        else
        {
            SetDifficulty((int)GameDifficulty.Madness);
            SetGameMode((int)GameMode.Survival);
        }  
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
                    if (currentWave < lvl1Waves.Length)
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
    void SpawnRandomEnemy(Wave wave,List<Transform> SpawnPoints)
    {
        int randEnemy = (int)Random.Range(0f, wave.enemies.Count);
        int randSpawnPoint = (int)Random.Range(0f, SpawnPoints.Count);
        float randSize = Random.Range(minEnemySize, maxEnemySize);

        if(wave.enemies[randEnemy].gameObject.name == "BossTomato" ||
            wave.enemies[randEnemy].gameObject.name == "BossSalchicha")
        {
            GameObject go1 = Instantiate(wave.enemies[randEnemy],
            BossSpawnPoint.position,wave.enemies[randEnemy].transform.rotation);

            go1.name = go1.name.Replace("(Clone)", "");
            spawnedEnemies.Add(go1);
        }
        else
        {
            GameObject go = Instantiate(wave.enemies[randEnemy],
            SpawnPoints[randSpawnPoint].transform.position,
            wave.enemies[randEnemy].transform.rotation);
            go.transform.localScale = new Vector3(randSize, randSize, randSize);

            go.name = go.name.Replace("(Clone)", "");

            SetEnemyDifficulty(go);
            spawnedEnemies.Add(go);
        }
    }

    IEnumerator SpawnWave(Wave wave, List<Transform> SpawnPoints)
    {
        for (int i = 0; i < wave.enemyAmount; i++)
        {
            if (gManager.gameStarted)
                SpawnRandomEnemy(wave,SpawnPoints);
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
            AkSoundEngine.PostEvent("gameplay_start_wave", gameObject);
            isSpawning = false;
            uiManager.SetWaveNumber(currentWave + 1);
            StartCoroutine(SpawnWave(lvl1Waves[currentWave],lvl1SpawnPoints));
            currentWave++;
            SetCurrentAudio();
        }
    }

    void EnemiesDieGameMode()
    {
        if ((spawnedEnemies.Count == 0) && isSpawning && (seconds <= 0))
        {
            AkSoundEngine.PostEvent("gameplay_start_wave", gameObject);
            isSpawning = false;

            userSurvivalRecord++;
            uiManager.SetWaveNumber(userSurvivalRecord);
            ScoreManager.Get().SetMaxWave(userSurvivalRecord);

            StartCoroutine(SpawnWave(lvl1Waves[currentWave],lvl1SpawnPoints));

            if (currentWave < maxSurvivalWave)
                currentWave++;
            SetCurrentAudio();
        }
    }

    void SurvivalGameMode()
    {
        if (seconds <= 0 && isSpawning)
        {
            AkSoundEngine.PostEvent("gameplay_start_wave", gameObject);
            isSpawning = false;

            userSurvivalRecord++;
            uiManager.SetWaveNumber(userSurvivalRecord);
            ScoreManager.Get().SetMaxWave(userSurvivalRecord);

            if (gameDifficulty == GameDifficulty.Madness)
                StartCoroutine(SpawnWave(lvl2Waves[currentWave], lvl2SpawnPoints));

            if (currentWave < maxSurvivalWave)
                currentWave++;
            SetCurrentAudio();
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

            go.GetComponent<NavMeshAgent>().speed = difficulties[(int)gameDifficulty].tomatoSpeed;
            go.GetComponent<Enemy>().damage = difficulties[(int)gameDifficulty].tomatoDmg;
            go.GetComponent<Enemy>().life = difficulties[(int)gameDifficulty].tomatoLife;
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

            if (seconds == 5)
            {
                uiManager.ActivePopUpAlert("WARNING!", "Next wave is coming");
            }
        }
    }

    public void SetCurrentAudio()
    {
        if((currentWave == 1) &&(sceneHandler.scene == SceneIndexes.LEVEL_1))
        {
            AkSoundEngine.SetState("MUSIC", "level_one_battle");
            AkSoundEngine.PostEvent("ui_battle_level_one", gameObject);
        }
        if ((currentWave == 1) && (sceneHandler.scene == SceneIndexes.LEVEL_2))
        {
            AkSoundEngine.SetState("MUSIC", "level_two_battle");
            AkSoundEngine.PostEvent("ui_battle_level_two", gameObject);
        }
    }

}
