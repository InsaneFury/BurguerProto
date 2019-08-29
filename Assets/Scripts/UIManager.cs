using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonobehaviourSingleton<UIManager>
{
    public Text dropSize;
    public Image healthBar;
    Player player;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        dropSize.text = "Drop Size: " + Gun.Get().bulletSize.ToString();
        player = Player.Get();
        healthBar.fillAmount = player.life / 100f;
    }

    public void RefreshUI()
    {
        dropSize.text = "Drop Size: " +  Gun.Get().bulletSize.ToString();
    }

    public void RefreshHealthbar()
    {
        if(healthBar.fillAmount != player.life)
        {
            healthBar.fillAmount = player.life / 100f;
        }
    }
}
