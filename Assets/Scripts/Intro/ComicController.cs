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
    void Start() => HideAll();
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ActiveNextBlock();
    }

    private void HideAll()
    {
        for (int i = 0; i < currentlayers.Count; i++)
            currentlayers[i].SetActive(false);
    }

    private void ActiveNextBlock()
    {
        if (currentBlock < currentlayers.Count)
        {
            currentlayers[currentBlock].SetActive(true);
            currentBlock++;
        }
    }

    private void LoadLayers()
    {
        ClosePage();
        for (int i = 0; i < pages[currentPage].transform.childCount; i++)
        {
            currentlayers.Add(pages[currentPage].transform.GetChild(i).gameObject);
        }
            
    }
    private void ClosePage()
    {
        if (currentlayers.Count > 0)
        {
            for (int i = 0; i < currentlayers.Count; i++)
            {
                currentlayers[i].GetComponent<TweenAnimation>().OnClose();
            }
            currentlayers.Clear();
        } 
    }
    private void ActiveNextPage()
    {
        currentPage++;
        LoadLayers();
    }
}
