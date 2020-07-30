using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGameSettings : MonobehaviourSingleton<MenuGameSettings>
{
    public GameModeSetting gameModeSettings;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        SetDefault();
    }

    public void SetDefault()
    {
        gameModeSettings.easy = true;
        gameModeSettings.nightmare = false;
        gameModeSettings.map1 = true;
        gameModeSettings.map2 = false;
    }
    public void SetHardDifficultyMode(bool difficulty) => gameModeSettings.nightmare = difficulty;
    public void SetEasyDifficultyMode(bool difficulty) => gameModeSettings.easy = difficulty;

    public void SetClassicMode(bool mapDefault) => gameModeSettings.map1 = mapDefault;
    public void SetMadnessMode(bool mapDefault) => gameModeSettings.map2 = mapDefault;

    public void SetSceneLoader()
    {
            ScenesManagerHandler.Get().scene = gameModeSettings.easy?SceneIndexes.LEVEL_1 : SceneIndexes.LEVEL_2;
    }
    public GameModeSetting GetSettings() => gameModeSettings;
    public void ShowSettings()
    {
        Debug.Log($"DifficultyEasy: {gameModeSettings.easy}");
        Debug.Log($"DifficultyHard: {gameModeSettings.nightmare}");
        Debug.Log($"Map1: {gameModeSettings.map1}");
        Debug.Log($"Map2: {gameModeSettings.map2}");
    }
}
