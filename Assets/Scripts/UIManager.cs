using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonobehaviourSingleton<UIManager>
{
    public Text dropSize;
    public GameObject healthBar;
    Player player;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        dropSize.text = "Drop Size: " + Gun.Get().bulletSize.ToString();
        player = Player.Get();
    }

    public void RefreshUI()
    {
        dropSize.text = "Drop Size: " +  Gun.Get().bulletSize.ToString();
    }

    public void RefreshHealthbar()
    {
        if(healthBar.transform.localScale.x != player.life)
        {
            healthBar.transform.localScale =  new Vector3(player.life / 100f, 1f, 1f);
        }
    }
}
