using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicController : MonoBehaviour
{
    public GameObject[] pages;
    private List<GameObject> currentlayers;
    private int currentBlock = 0;
    private int currentPage = 0;
    private void Awake()
    {
        currentlayers = new List<GameObject>();
        LoadLayers();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ActiveNextBlock();
    }

    private void HideAll()
    {
        for (int i = 0; i < currentlayers.Count; i++)
            currentlayers[i].SetActive(false);
        currentlayers[0].SetActive(true);
        currentBlock++;
    }

    private void ActiveNextBlock()
    {
        if (currentBlock < currentlayers.Count)
        {
            currentlayers[currentBlock].SetActive(true);
            currentBlock++;
        }
        else if (currentBlock >= currentlayers.Count)
        {
            currentBlock = 0;
            ClosePage();
            currentPage++;
            if (currentPage > 1)
            {
                ScenesManagerHandler.Get().LoadSceneHandler((int)SceneIndexes.MENU);
                return;
            }
            LoadLayers();
        }
            
    }

    private void LoadLayers()
    {
        currentlayers.Clear();
        for (int i = 0; i < pages[currentPage].transform.childCount; i++)
        {
            currentlayers.Add(pages[currentPage].transform.GetChild(i).gameObject);
        }
        HideAll();
    }
    private void ClosePage()
    {
        if (currentlayers.Count > 0)
        {
            pages[currentPage].GetComponent<TweenAnimation>().OnClose();
        } 
    }
}
