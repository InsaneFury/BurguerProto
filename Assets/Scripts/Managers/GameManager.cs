using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonobehaviourSingleton<GameManager>
{
    Player player;

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
        gameStarted = true;
    }

    void Update()
    {
        if (!player.isAlive)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        gameStarted = false;
        gameOverText.SetActive(true);
        cg.saturation.value = new FloatParameter() { value = gameOverSaturation };
    }
}
