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
    public float loadingFakeTime = 5f;

    [Space]
    [Header("Tips Settings")]
    public TextMeshProUGUI tipsText;
    public CanvasGroup alphaCanvas;
    public string[] tips;
    public float tipsIntervaleTime = 1f;

    private float timeLoading = 0f;
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

    public IEnumerator GetSceneLoadProgress()
    {
        totalSceneProgress = 0f;
        timeLoading = 0f;

        for (int i = 0; i < scenesLoading.Count; i++)
        {
            scenesLoading[i].allowSceneActivation = false;
            while (!scenesLoading[i].isDone)
            {
                timeLoading += Time.deltaTime;

                foreach (AsyncOperation operation in scenesLoading)
                    totalSceneProgress += operation.progress;

                totalSceneProgress = (totalSceneProgress / scenesLoading.Count) + 0.1f;
                totalSceneProgress = (totalSceneProgress * timeLoading) / loadingFakeTime;

                loadingPercentage.text = $"{Mathf.RoundToInt(totalSceneProgress * 100f)}%";
                progressBar.fillAmount = totalSceneProgress;

                //Loading Complete
                if (totalSceneProgress >= 1f)
                {
                    for (int j = 0; j < scenesLoading.Count; j++)
                    {
                        scenesLoading[j].allowSceneActivation = true;
                    }
                }
                yield return null;
            }
        }
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
