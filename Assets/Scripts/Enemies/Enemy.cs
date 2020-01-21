using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using System;

public class Enemy : MonoBehaviour
{
    [Header("Settings")]
    public float damage = 1;
    public NavMeshAgent enemyAgent;
    public float attackDistance = 2;
    public float life = 100f;
    public Image healthBar;
    public GameObject mat;
    public GameObject popUp;
    public float timeToDie = 5f;
    public SphereCollider enemyCollider;

    [Header("Drops")]
    public GameObject soul;
    public GameObject[] itemsToDrop;
    public int bulletDropRate = 50;
    public int granadeDropRate = 10;
    public GameObject spawnPos;

    [Header("VFX")]
    public float flashTime = 0.1f;

    public static event Action<Enemy> OnDieAction;

    protected bool isAlive = true;

    private void Start()
    {
        enemyCollider = GetComponent<SphereCollider>();
        enemyAgent = GetComponent<NavMeshAgent>();

        healthBar.fillAmount = life / 100f;
    }

    protected virtual void TakeDamage(int dmg)
    {
        if (popUp && life > 0)
        {
            ShowPopUp(dmg);
            //Audio
            AkSoundEngine.PostEvent("Damage_enemigos", gameObject);
        }
        if (life > 0)
        {
            StartCoroutine("DamageFeedback");
            life -= dmg;
        }
        
        RefreshHealthbar();
    }

    protected virtual void Attack() { }

    protected virtual void Die()
    {
        isAlive = false;
        if (OnDieAction != null)
            OnDieAction(this);
        
    }

    private void RefreshHealthbar()
    {
        if (healthBar.fillAmount != life)
        {
            healthBar.fillAmount = life / 100f;
        }
    }

    private IEnumerator DamageFeedback()
    {
        //mat.gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(flashTime);
       // mat.gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
    }

    private void ShowPopUp(int dmg)
    {
        GameObject go = Instantiate(popUp, transform.position, Quaternion.identity, transform);
        go.GetComponent<TextMeshPro>().text = dmg.ToString();
    }

}
