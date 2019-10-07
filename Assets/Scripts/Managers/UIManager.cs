using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonobehaviourSingleton<UIManager>
{
    [Header("Player HUD")]
    public Image healthBar;
    public Image sizeBar;
    public Image souls;
    public Image healthSkill;
    public Image dashSkill;

    [Header("HUD Settings")]
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
        RefreshSouls();
    }

    private void Update()
    {
        RefreshSkillsIcons();
    }

    public void RefreshSouls()
    {
        souls.fillAmount = player.soulsCollected / 100f;
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

    public void RefreshSkillsIcons()
    {
        if (player.isDashing || (player.soulsCollected <= 0))
        {
            dashSkill.fillAmount = 0;
        }
        else 
        {
            dashSkill.fillAmount = 1;
        }
        if (player.soulsCollected < player.healCost)
        {
            healthSkill.fillAmount = 0;
        }
        else
        {
            healthSkill.fillAmount = 1;
        }

    }
}
