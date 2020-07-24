using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenesManagerHandler : MonoBehaviour
{
    LoadingScenesManager loadingManager;

    private void Awake()
    {
        loadingManager = FindObjectOfType<LoadingScenesManager>();
    }
    public void LoadSceneHandler()
    {
        loadingManager.LoadGame();
    }
}
