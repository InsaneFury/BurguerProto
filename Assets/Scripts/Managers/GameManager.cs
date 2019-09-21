using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonoBehaviour
{
    Player player;

    [Header("GameOver")]
    public GameObject gameOverText;
    public PostProcessProfile profile;
    public FloatParameter gameOverSaturation;
    ColorGrading cg;

    void Start()
    {
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

    void GameOver()
    {
        gameOverText.SetActive(true);
        cg.saturation.value = new FloatParameter() { value = gameOverSaturation };
    }
}
