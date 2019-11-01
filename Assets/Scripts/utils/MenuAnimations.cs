using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimations : MonoBehaviour
{
    public GameObject difficulties;
    public GameObject init;

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
}
