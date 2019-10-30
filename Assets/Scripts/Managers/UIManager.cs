using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonobehaviourSingleton<UIManager>
{
    [Header("Player HUD")]
    public Image healthBar;
    public Image souls;
    public Image healthSkill;
    public Image dashSkill;

    [Header("HUD Settings")]
    public GameObject inGameHUD;
    public GameObject menuHUD;
    public float soulFillvelocity = 0.1f;
    public TextMeshProUGUI version;

    [Header("Pause Settings")]
    public GameObject pauseText;

    [Header("Wave Time Settings")]
    public GameObject waveWithTimer;
    public TextMeshProUGUI waveTimer;

    [Header("Wave Enemies Settings")]
    public GameObject waveWithEnemies;
    public TextMeshProUGUI waveEnemiesAlive;

    [Header("Waves Completed")]
    public GameObject waveCompleted;

    [Header("Cameras")]
    public GameObject VCamThirdPerson;
    public GameObject VCamMenu;

    Player player;
    EnemySpawner eSpawner;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        VCamThirdPerson.SetActive(false);
        inGameHUD.SetActive(false);
        menuHUD.SetActive(true);
        player = Player.Get();
        healthBar.fillAmount = player.life / 100f;
        RefreshSouls();
        eSpawner = EnemySpawner.Get();
        ShowWaveInfo(); 
    }

    private void Update()
    {
        RefreshSkillsIcons();
        RefreshWaveInfo();
    }

    public void ActiveInGameUI()
    {
        inGameHUD.SetActive(true);
        VCamThirdPerson.SetActive(true);
        menuHUD.SetActive(false);
        VCamMenu.SetActive(false);
    }

    #region Refresh UI
    public void RefreshSouls()
    {
        souls.fillAmount = player.soulsCollected / 100f;
    }

    public void RefreshHealthbar()
    {
        if (healthBar.fillAmount != player.life)
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

    #endregion

    #region Wave info
    void ShowWaveInfo()
    {
        if(eSpawner != null)
        {
            waveCompleted.SetActive(false);
            if (eSpawner.nextWaveIfEmpty)
            {
                waveEnemiesAlive.text = eSpawner.spawnedEnemies.Count.ToString();
                waveWithEnemies.SetActive(true);
                waveWithTimer.SetActive(false);
            }
            else
            {
                waveTimer.text = "00:" + eSpawner.seconds.ToString();
                waveWithEnemies.SetActive(false);
                waveWithTimer.SetActive(true);
            }
        }
        
    }

    void RefreshWaveInfo()
    {
        if (eSpawner != null)
        {
            if (eSpawner.nextWaveIfEmpty)
            {
                waveEnemiesAlive.text = "0" + eSpawner.spawnedEnemies.Count;
            }
            else
            {
                waveTimer.text = "00:" + eSpawner.seconds.ToString();
            }

            if (eSpawner.allWavesCompleted)
            {
                waveWithEnemies.SetActive(false);
                waveWithTimer.SetActive(false);
                waveCompleted.SetActive(true);
            }
        }
    }
    #endregion

}
