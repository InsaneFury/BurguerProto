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
    public GameObject spawnPos;

    [Header("VFX")]
    public float flashTime = 0.1f;

    public static event Action<Enemy> OnDieAction;

    Player player;
    Animator anim;
    Rigidbody rb;
    bool isAlive = true;
    
    float distance = 0;

    enum EnemyAction : short {Idle = 0,Run,Attack};
    EnemyAction actions;

    private void Start()
    {
        player = Player.Get();
        enemyCollider = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        enemyAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        healthBar.fillAmount = life / 100f;
        actions = EnemyAction.Run;
    }
    private void Update()
    {
        if (isAlive)
        {
            distance = Vector3.Distance(transform.position, player.transform.position);
            switch (actions)
            {
                case EnemyAction.Idle:
                    break;
                case EnemyAction.Run:
                    enemyAgent.SetDestination(player.transform.position);

                    if (distance < attackDistance)
                    {
                        actions = EnemyAction.Attack;
                    }
                    break;
                case EnemyAction.Attack:
                    anim.SetTrigger("attack");
                    break;
            }
        }
    }

    public void Chase()
    {
        actions = EnemyAction.Run;
    }

    public void CheckIfCanAttack()
    {
        if (distance < attackDistance)
        {
            player.TakeDamage(damage);
        }
        else
        {
            Chase();
        }
    }

    public void TakeDamage(int dmg)
    {
        if (popUp && life > 0)
        {
            ShowPopUp(dmg);
        }
        
        if (life > 0)
        {
            StartCoroutine("DamageFeedback");
            life -= dmg;
        }
        if((life <= 0) && !enemyCollider.isTrigger)
        {
            enemyCollider.isTrigger = true;
            Die();
        }
        RefreshHealthbar();
    }

    void Die()
    {
        isAlive = false;
        anim.SetBool("die", true);
        if (OnDieAction != null)
            OnDieAction(this);
    }

    public void DeleteTomato()
    {
        Drop();
        EnemySpawner.Get().spawnedEnemies.Remove(gameObject);
        Destroy(gameObject, timeToDie);
    }

    void RefreshHealthbar()
    {
        if (healthBar.fillAmount != life)
        {
            healthBar.fillAmount = life / 100f;
        }
    }

    IEnumerator DamageFeedback()
    {
        //mat.gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(flashTime);
       // mat.gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
    }

    void Drop()
    {
        Instantiate(soul, spawnPos.transform.position, Quaternion.identity);
    }

    void ShowPopUp(int dmg)
    {
        GameObject go = Instantiate(popUp, transform.position, Quaternion.identity, transform);
        go.GetComponent<TextMeshPro>().text = dmg.ToString();
    }
}
