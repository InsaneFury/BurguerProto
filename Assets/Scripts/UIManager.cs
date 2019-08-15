using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonobehaviourSingleton<UIManager>
{
    public Text dropSize;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        dropSize.text = "Drop Size: " + Gun.Get().bulletSize.ToString();   
    }

    public void RefreshUI()
    {
        dropSize.text = "Drop Size: " +  Gun.Get().bulletSize.ToString();
    }
}
