﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonobehaviourSingleton<UIManager>
{
    public Image healthBar;
    public Image sizeBar;
    public TextMeshProUGUI soul;
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

    public void RefreshUI()
    {
        soul.text = player.soulsCollected.ToString();
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