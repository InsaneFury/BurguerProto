using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameManager : MonobehaviourSingleton<GameManager>
{
    Player player;

    [Header("Timelines")]
    public PlayableDirector menuToGameplay;
    public PlayableDirector menuToCredits;
    public PlayableDirector creditsToMenu;

    [Header("Player settings")]
    public Transform startPosition;

    [Header("Game Settings")]
    public bool gameStarted;
    public bool pause;

    [Header("GameOver")]
    public GameObject gameOverText;
    public PostProcessProfile profile;
    public FloatParameter gameOverSaturation;
    ColorGrading cg;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        UIManager.Get().version.text ="v"+ Application.version;
        player = Player.Get();
        profile.TryGetSettings(out cg);
        cg.saturation.value = new FloatParameter() { value = 0 };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gameStarted)
        {
            PauseGame();
        }
        if (!player.isAlive)
        {
            GameOver();
        }
    }


    #region Timelines
    public void GameStart()
    {
        menuToGameplay.Play();
    }

    public void GoToCredits()
    {
        menuToCredits.Play();
    }

    public void GoToMenu()
    {
        creditsToMenu.Play();
    }
    #endregion

    public void ActiveGame()
    {
        gameStarted = true;
        UIManager.Get().ActiveInGameUI();
    }

    void GameOver()
    {
        gameStarted = false;
        gameOverText.SetActive(true);
        cg.saturation.value = new FloatParameter() { value = gameOverSaturation };
    }

    void PauseGame()
    {
        pause = !pause;
        UIManager.Get().pause.SetActive(pause);
        Time.timeScale = pause ? 0 : 1;
    }

    public void RestartGame()
    {
        EnemySpawner.Get().ResetSpawner();
        ResetPlayer();
        cg.saturation.value = new FloatParameter() { value = 0 };
        gameStarted = true;
        gameOverText.SetActive(false);
        ScoreManager.Get().score = 0;
        ScoreManager.Get().enemiesKilled = 0;
        if(pause)
        PauseGame();
    }

    public void Menu()
    {
        if (pause)
            PauseGame();
        UIManager.Get().initMenu.SetActive(true);
        SceneManager.LoadScene("Gameplay");
    }

    void ResetPlayer()
    {
        player.transform.position = startPosition.position;
        player.transform.rotation = startPosition.rotation;
        player.ResetStats();
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
}
