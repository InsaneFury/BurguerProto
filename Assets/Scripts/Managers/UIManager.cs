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
    public GameObject infinitSymbol;
    public TextMeshProUGUI currentAmmo;
    public GameObject currentAmmoGo;

    [Header("HUD Settings")]
    public GameObject inGameHUD;
    public float soulFillvelocity = 0.1f;

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

    [Header("GameMode")]
    public TextMeshProUGUI gameModePauseText;
    public TextMeshProUGUI gameOverModeText;
    public Image pauseImage;
    public Image gameOverImage;
    public Sprite[] sprites;
    public Sprite[] spritesColor;

    [Header("GameOver")]
    public TextMeshProUGUI enemiesKilledText;
    public TextMeshProUGUI maxWaves;
    public TextMeshProUGUI maxScore;

    [Header("PopUpAlert")]
    public GameObject popUpAlert;
    public TextMeshProUGUI popUpTitle;
    public TextMeshProUGUI popUpText;

    private Player player;
    private EnemySpawner enemySpawner;
    private ScoreManager scoreManager;
    private MachineGun machineGun;
    private Granade granade;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        player = Player.Get();
        machineGun = MachineGun.Get();
        granade = Granade.Get();
        enemySpawner = EnemySpawner.Get();
        scoreManager = ScoreManager.Get();

        Player.OnChangeWeapon += SetActiveWeaponUI;
        popUpAlert.SetActive(false);
        healthBar.fillAmount = player.life / 100f;
        RefreshSouls();
        ShowWaveInfo();
        currentAmmoGo.SetActive(false);
    }

    private void Update()
    {
        RefreshSkillsIcons();
        RefreshWaveInfo();
        RefreshStats();
        RefreshHealthbar();
        RefreshSouls();
    }
    #region WeaponsUI
    public void SetActiveWeaponUI(Player p)
    {
        if (player.currentActiveWeapon > 2)
            player.currentActiveWeapon = 0;
        for (int i = 0; i < weaponsUI.Length; i++)
        {
            weaponsUI[i].GetComponent<Animator>().SetBool("weaponActive", false);
        }
        weaponsUI[player.currentActiveWeapon].GetComponent<Animator>().SetBool("weaponActive", true);
        currentWeaponText.text = weaponsNames[player.currentActiveWeapon];
    }
    #endregion
    #region Refresh UI
    public void RefreshStats()
    {
        score.text = scoreManager.score.ToString();
        enemiesKilled.text = scoreManager.enemiesKilled.ToString();
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
        int dashFillAmount = player.isDashing || (player.soulsCollected < player.dashCost) ? 0 : 1;
        dashSkill.fillAmount = dashFillAmount;

        int healtFillAmount = (player.soulsCollected < player.healCost) ? 0 : 1;
        healthSkill.fillAmount = healtFillAmount;
    }
    #endregion
    #region Wave info
    void ShowWaveInfo()
    {
        if (enemySpawner != null)
        {
            waveCompleted.SetActive(false);
            if (enemySpawner.noTimerMode)
            {
                waveEnemiesAlive.text = enemySpawner.spawnedEnemies.Count.ToString();
                waveWithEnemies.SetActive(true);
                waveWithTimer.SetActive(false);
            }
            else
            {
                waveTimer.text = enemySpawner.seconds.ToString();
                waveWithEnemies.SetActive(false);
                waveWithTimer.SetActive(true);
            }
        }
    }
    void RefreshWaveInfo()
    {
        if (!enemySpawner) return;
        if (enemySpawner.noTimerMode)
            waveEnemiesAlive.text = enemySpawner.spawnedEnemies.Count.ToString();
        else
            waveTimer.text = enemySpawner.seconds.ToString();

        if (enemySpawner.allWavesCompleted)
        {
            waveWithEnemies.SetActive(false);
            waveWithTimer.SetActive(false);
            waveCompleted.SetActive(true);
        }
    }
    public void SetWaveNumber(int num)
    {
        waveNumber.GetComponent<TextMeshProUGUI>().text = "WAVE " + num.ToString();
        StartCoroutine(StartWaveNumber());    
    }
    private IEnumerator StartWaveNumber()
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
    public void SetRoomGameModeImage(int img)
    {
        pauseImage.sprite = sprites[img];
    }
    public void SetRoomGameModeImageColor(int img)
    {
        gameOverImage.sprite = spritesColor[img];
    }
    public void SetGameOverResults(int enemiesKilled,int maxwaves,int score)
    {
        enemiesKilledText.text = enemiesKilled.ToString();
        maxWaves.text = maxwaves.ToString();
        maxScore.text = score.ToString();
    }
    #endregion

}
