using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    public GameObject[] settingsMenus;

    private void Start() => ResetAll();
    public void ActiveMenu(int activeMenu) => SetAllSettingsMenus(activeMenu);
    public void ResetAll()
    {
        for (int i = 0; i < settingsMenus.Length; i++)
        {
            if (settingsMenus[i].GetComponent<TweenAnimation>())
                settingsMenus[i].GetComponent<TweenAnimation>().OnClose();
            else
                settingsMenus[i].SetActive(false);
        }
    }
    public void SetAllSettingsMenus(int activeMenu = 0)
    {
        for (int i = 0; i < settingsMenus.Length; i++)
        {
            if ((i == activeMenu))
                settingsMenus[i].SetActive(true);
            else
                settingsMenus[i].GetComponent<TweenAnimation>().OnClose();
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
