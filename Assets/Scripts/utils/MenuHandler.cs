﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    public GameObject[] settingsMenus;

    private void Start() => ResetAll();
    public void ActiveMenu(int activeMenu) => SetAllSettingsMenus(activeMenu);
    public void ResetAll() => SetAllSettingsMenus(0, true);
    private void SetAllSettingsMenus(int activeMenu = 0, bool hideAll = false)
    {
        for (int i = 0; i < settingsMenus.Length; i++)
        {
            settingsMenus[i].SetActive(false);
            if ((i == activeMenu) && !hideAll)
                settingsMenus[i].SetActive(true);
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
    Application.Quit();
#endif
    }
}