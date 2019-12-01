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
    public TextMeshProUGUI lifeNum;
    public TextMeshProUGUI SoulNum;

    [Header("Weapons UI")]
    public GameObject[] weaponsUI;
    public string[] weaponsNames;
    public TextMeshProUGUI currentWeaponText;

    [Header("HUD Settings")]
    public GameObject inGameHUD;
    public GameObject menuHUD;
    public GameObject initMenu;
    public GameObject difficultyUi;
    public float soulFillvelocity = 0.1f;
    public TextMeshProUGUI version;

    [Header("Score Settings")]
    public TextMeshProUGUI score;
    public TextMeshProUGUI enemiesKilled;

    [Header("Pause Settings")]
    public GameObject pause;

    [Header("Wave Time Settings")]
    public GameObject waveWithTimer;
    public TextMeshProUGUI waveTimer;

    [Header("Wave Enemies Settings")]
    public GameObject waveWithEnemies;
    public TextMeshProUGUI waveEnemiesAlive;

    [Header("Waves Completed")]
    public GameObject waveCompleted;
    public GameObject waveNumber;
    private float waveNumberTime = 3f;

    [Header("Cameras")]
    public GameObject VCamThirdPerson;
    public GameObject VCamMenu;

    [Header("PopUpAlert")]
    public GameObject popUpAlert;
    public TextMeshProUGUI popUpTitle;
    public TextMeshProUGUI popUpText;

    Player player;
    EnemySpawner eSpawner;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        Player.OnChangeWeapon += SetActiveWeaponUI;
        popUpAlert.SetActive(false);
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
        RefreshStats();
        RefreshHealthbar();
        RefreshSouls();
    }

    public void ActiveInGameUI()
    {
        inGameHUD.SetActive(true);
        VCamThirdPerson.SetActive(true);
        menuHUD.SetActive(false);
        VCamMenu.SetActive(false);
    }

    #region WeaponsUI
    public void SetActiveWeaponUI(Player p)
    {
        for (int i = 0; i < weaponsUI.Length; i++)
        {
            weaponsUI[i].GetComponent<Animator>().SetBool("weaponActive", false);
        }
        weaponsUI[player.currentActiveWeapon].GetComponent<Animator>().SetBool("weaponActive", true);
        currentWeaponText.text = weaponsNames[player.currentActiveWeapon];
        Debug.Log("Funca");
    }
    #endregion

    #region Refresh UI

    public void RefreshStats()
    {
        score.text = ScoreManager.Get().score.ToString();
        enemiesKilled.text = ScoreManager.Get().enemiesKilled.ToString();
    }

    public void RefreshSouls()
    {
        souls.fillAmount = player.soulsCollected / 100f;
        SoulNum.text = ((int)player.soulsCollected).ToString();
    }

    public void RefreshHealthbar()
    {
        if (healthBar.fillAmount != player.life)
        {
            healthBar.fillAmount = player.life / 100f;
            lifeNum.text = ((int)player.life).ToString();
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
        if (eSpawner != null)
        {
            waveCompleted.SetActive(false);
            if (eSpawner.noTimerMode)
            {
                waveEnemiesAlive.text = eSpawner.spawnedEnemies.Count.ToString();
                waveWithEnemies.SetActive(true);
                waveWithTimer.SetActive(false);
            }
            else
            {
                waveTimer.text = eSpawner.seconds.ToString();
                waveWithEnemies.SetActive(false);
                waveWithTimer.SetActive(true);
            }
        }

    }

    void RefreshWaveInfo()
    {
        if (eSpawner != null)
        {
            if (eSpawner.noTimerMode)
            {
                waveEnemiesAlive.text = eSpawner.spawnedEnemies.Count.ToString();
            }
            else
            {
                waveTimer.text = eSpawner.seconds.ToString();
            }

            if (eSpawner.allWavesCompleted)
            {
                waveWithEnemies.SetActive(false);
                waveWithTimer.SetActive(false);
                waveCompleted.SetActive(true);
            }
        }
    }

    public void SetWaveNumber(int num)
    {
        waveNumber.GetComponent<TextMeshProUGUI>().text = "WAVE " + num.ToString();
        StartCoroutine(StartWaveNumber());
        
    }

    IEnumerator StartWaveNumber()
    {
        waveNumber.SetActive(true);
        yield return new WaitForSecondsRealtime(waveNumberTime);
        waveNumber.SetActive(false);
        EnemySpawner.Get().ResetTimer();
    }

    public void ActivePopUpAlert(string popTitle,string popText)
    {
        popUpAlert.SetActive(false);
        popUpTitle.text = popTitle;
        popUpText.text = popText;
        popUpAlert.SetActive(true);
    }
    #endregion

}
