using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Playables;

public class GameManager : MonobehaviourSingleton<GameManager>
{
    Player player;

    public PlayableDirector playableDirector;

    [Header("Game Settings")]
    public bool gameStarted;

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
        if (!player.isAlive)
        {
            GameOver();
        }
    }

    public void GameStart()
    {
        playableDirector.Play();
    }

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
}
