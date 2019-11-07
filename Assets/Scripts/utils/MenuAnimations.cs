using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuAnimations : MonoBehaviour
{
    public GameObject difficulties;
    public GameObject init;
    public GameObject controls;

    public void DeactiveDifficulties()
    {
        difficulties.SetActive(false);
    }

    public void ActiveDifficulties()
    {
        difficulties.SetActive(true);

    }

    public void DeactiveInit()
    {
        init.SetActive(false);
    }

    public void ActiveInit()
    {
        init.SetActive(true);
    }

    public void DeactiveControls()
    {
        controls.SetActive(false);
    }

    public void ActiveControls()
    {
        controls.SetActive(true);
    }
}
