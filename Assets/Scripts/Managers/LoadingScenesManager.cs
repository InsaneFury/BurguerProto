using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public partial class LoadingScenesManager : MonobehaviourSingleton<LoadingScenesManager>
{
    [Header("Screen Settings")]
    public GameObject loadingScreen;
    public Image progressBar;
    public TextMeshProUGUI loadingPercentage;

    [Space]
    [Header("Tips Settings")]
    public TextMeshProUGUI tipsText;
    public CanvasGroup alphaCanvas;
    public string[] tips;
    public float tipsIntervaleTime = 1f;
    public float fakeWaitLoadingTime = 3f;

    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    private float totalSceneProgress;
    private int tipCount;
    private SceneIndexes currentSceneLoaded;
    public override void Awake() => base.Awake();

    void Start() => StartGameIntro();
    
    void StartGameIntro()
    {
        currentSceneLoaded = SceneIndexes.INTRO;
        SceneManager.LoadSceneAsync((int)SceneIndexes.INTRO, LoadSceneMode.Additive);
    }

    public void LoadScene(SceneIndexes sceneToLoad)
    {
        loadingPercentage.text = "0%";
        loadingScreen.gameObject.SetActive(true);
        StartCoroutine(GenerateTips());

        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)currentSceneLoaded));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)sceneToLoad, LoadSceneMode.Additive));
        currentSceneLoaded = sceneToLoad;
        StartCoroutine(GetSceneLoadProgress());
    }

    public void LoadGame()
    {
        loadingScreen.gameObject.SetActive(true);
        StartCoroutine(GenerateTips());

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
                //0.9f is a value default, cuz' unity asyncOperations load max to 0.9f and then use the 0.1f to active.
                totalSceneProgress = Mathf.Clamp01(totalSceneProgress / 0.9f);
                loadingPercentage.text = $"{Mathf.RoundToInt(totalSceneProgress * 100f)}%";
                progressBar.fillAmount = totalSceneProgress;
                yield return null;
            }
        }

        yield return new WaitForSeconds(fakeWaitLoadingTime);
        loadingScreen.gameObject.SetActive(false);
    }

    public IEnumerator GenerateTips()
    {
        tipCount = Random.Range(0, tips.Length);
        tipsText.text = tips[tipCount];
        while (loadingScreen.activeInHierarchy)
        {
            yield return new WaitForSeconds(tipsIntervaleTime);
            LeanTween.alphaCanvas(alphaCanvas, 0, 0.5f);

            yield return new WaitForSeconds(0.5f);

            tipCount++;
            if (tipCount >= tips.Length)
                tipCount = 0;

            tipsText.text = tips[tipCount];

            LeanTween.alphaCanvas(alphaCanvas, 1.0f, 0.5f);
        }
    }
}
