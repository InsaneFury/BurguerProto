using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonobehaviourSingleton<GameManager>
{

    [Header("Player settings")]
    public Transform startPosition;

    [Header("Game Settings")]
    public bool gameStarted;
    public bool pause;

    [Header("GameOver")]
    public GameObject gameOverText;
    public Volume postProcessingProfile;
    public ClampedFloatParameter saturation;

    private ColorAdjustments colorAdjustments;
    private float gameOverColorSaturation = -100f;
    private ScoreManager scoreManager;
    private Player player;
    private EnemySpawner enemySpawner;
    private UIManager uiManager;
    private ScenesManagerHandler sceneHandler;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        player = Player.Get();
        player.inputAction.GameManagerControls.Pause.performed += ctx => PauseGame();
        scoreManager = ScoreManager.Get();
        enemySpawner = EnemySpawner.Get();
        uiManager = UIManager.Get();
        Player.OnPlayerDead += GameOver;
        postProcessingProfile.profile.TryGet(out colorAdjustments);
        colorAdjustments.saturation.value = 0f;
        ActiveGame();
    }

    public void ActiveGame()
    {
        gameStarted = true;
        player.canPlay = true;
        //AkSoundEngine.PostEvent("Inicio_gameplay", gameObject);
    }

    public void GameOver(Player p)
    {
        //AkSoundEngine.PostEvent("Perder", gameObject);
        gameStarted = false;
        player.canPlay = false;
        gameOverText.SetActive(true);
        colorAdjustments.saturation.value = gameOverColorSaturation;
        enemySpawner.ResetSpawner();
        uiManager.SetGameOverResults(scoreManager.enemiesKilled, scoreManager.maxWave, scoreManager.score);
    }
    void PauseGame()
    {
        if (gameStarted)
        {
            pause = !pause;
            Time.timeScale = pause ? 0 : 1;
            uiManager.pause.SetActive(pause);

            if (pause)
            {
                player.canPlay = !pause;
                //AkSoundEngine.PostEvent("Pausa_ON", gameObject);
            }
            else
            {
                player.canPlay = pause;
                //AkSoundEngine.PostEvent("Pausa_OFF", gameObject);
            }
        }
    }
    public void RestartGame()
    {
        if(pause)
        PauseGame();
        colorAdjustments.saturation.value = 0f;
        gameOverText.SetActive(false);
        scoreManager.score = 0;
        scoreManager.enemiesKilled = 0;
        enemySpawner.ResetSpawner();
        ResetPlayer();
        ActiveGame();
        //AkSoundEngine.PostEvent("Restart", gameObject);
    }
    public void Menu()
    {
        if (pause)
            PauseGame();
        sceneHandler.LoadSceneHandler((int)SceneIndexes.MENU);
    }
    private void ResetPlayer()
    {
        player.transform.position = startPosition.position;
        player.transform.rotation = startPosition.rotation;
        player.ResetStats();
        player.canPlay = true;
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
#if UNITY_STANDALONE && !UNITY_EDITOR
        Application.Quit();
#endif
    }
    private void OnDestroy()
    {
        Player.OnPlayerDead -= GameOver;
    }
}
