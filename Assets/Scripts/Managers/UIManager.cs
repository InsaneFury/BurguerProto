using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonobehaviourSingleton<UIManager>
{
    public Image healthBar;
    public Image sizeBar;
    public Image souls;
    public float soulFillvelocity = 0.1f;
    Player player;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        player = Player.Get();
        healthBar.fillAmount = player.life / 100f;
        sizeBar.fillAmount = 0;
    }

    public void RefreshSouls()
    {
        if(souls.fillAmount < 1)
        {
            souls.fillAmount = player.soulsCollected / 100f;
        }      
    }

    public void RefreshSizeBar()
    {
        sizeBar.fillAmount = Gun.Get().bulletSize;
    }

    public void RefreshHealthbar()
    {
        if(healthBar.fillAmount != player.life)
        {
            healthBar.fillAmount = player.life / 100f;
        }
    }
}
