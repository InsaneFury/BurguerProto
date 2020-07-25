using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
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
    public Volume postProcessingProfile;
    public ClampedFloatParameter saturation;

    private ColorAdjustments colorAdjustments;
    private float gameOverColorSaturation = -100f;
    private ScoreManager sManager;

    public override void Awake()
    {
        base.Awake();
        player = Player.Get();
        player.inputAction.GameManagerControls.Pause.performed += ctx => PauseGame();
    }

    void Start()
    {
        sManager = ScoreManager.Get();
        //Audio
        //AkSoundEngine.PostEvent("Menu", gameObject);
        UIManager.Get().version.text ="v"+ Application.version;

        postProcessingProfile.profile.TryGet(out colorAdjustments);
        colorAdjustments.saturation.value = 0f;
    }

    void Update()
    {
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
        player.canPlay = true;
        //Audio
        //AkSoundEngine.PostEvent("Inicio_gameplay", gameObject);
        if (JukeBoxAudio.Get())
        {
            JukeBoxAudio.Get().PlayJukeBoxAudio();
        }
        UIManager.Get().ActiveInGameUI();
    }

    void GameOver()
    {
        //Audio
        AkSoundEngine.PostEvent("Perder", gameObject);
        if (JukeBoxAudio.Get())
        {
            JukeBoxAudio.Get().StopJukeBoxAudio();
        }
        gameStarted = false;
        player.canPlay = false;
        gameOverText.SetActive(true);
        colorAdjustments.saturation.value = gameOverColorSaturation;
        EnemySpawner.Get().ResetSpawner();
        UIManager.Get().SetGameOverResults(sManager.enemiesKilled, sManager.maxWave, sManager.score);
    }

    void PauseGame()
    {
        if (gameStarted)
        {
            pause = !pause;
            UIManager.Get().pause.SetActive(pause);
            Time.timeScale = pause ? 0 : 1;

            if (pause)
            {
                //Audio
                player.canPlay = false;
                AkSoundEngine.PostEvent("Pausa_ON", gameObject);
            }
            else
            {
                player.canPlay = true;
                //Audio
                AkSoundEngine.PostEvent("Pausa_OFF", gameObject);
            }
        }
    }

    public void RestartGame()
    {
        EnemySpawner.Get().ResetSpawner();
        ResetPlayer();
        colorAdjustments.saturation.value = 0f;
        ActiveGame();
        gameOverText.SetActive(false);
        ScoreManager.Get().score = 0;
        ScoreManager.Get().enemiesKilled = 0;
        if(pause)
        PauseGame();

        //Audio
        AkSoundEngine.PostEvent("Restart", gameObject);
    }

    public void Menu()
    {
        if (pause)
            PauseGame();
        UIManager.Get().initMenu.SetActive(true);

        //Sacar y cambiar por volver al menu sin recagar la scene.
        SceneManager.LoadScene("Gameplay");
    }

    void ResetPlayer()
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
}
