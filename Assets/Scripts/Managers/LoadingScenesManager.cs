using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public partial class LoadingScenesManager : MonobehaviourSingleton<LoadingScenesManager>
{
    public GameObject loadingScreen;
    public Image progressBar;

    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    private float totalSceneProgress;
    public override void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        SceneManager.LoadSceneAsync((int)SceneIndexes.MENU, LoadSceneMode.Additive);
    }

    void Update()
    {
        
    }

    public void LoadGame()
    {
        loadingScreen.gameObject.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.MENU));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.LEVEL_1, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    public IEnumerator GetSceneLoadProgress()
    {
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                totalSceneProgress = 0f;

                foreach (AsyncOperation operation in scenesLoading)
                {
                    totalSceneProgress += operation.progress;
                }

                totalSceneProgress = (totalSceneProgress / scenesLoading.Count);
                progressBar.fillAmount = totalSceneProgress;
                yield return null;
            }
        }
        loadingScreen.gameObject.SetActive(false);
    }
}
