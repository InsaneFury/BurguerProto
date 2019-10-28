using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

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

    [Header("Drops")]
    public GameObject soul;

    [Header("VFX")]
    public float flashTime = 0.1f;

    Player player;
    Animator anim;
    Rigidbody rb;
    
    float distance = 0;

    enum EnemyAction : short {Idle = 0,Run,Attack};
    EnemyAction actions;

    private void Start()
    {
        player = Player.Get();
        rb = GetComponent<Rigidbody>();
        enemyAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        healthBar.fillAmount = life / 100f;
        actions = EnemyAction.Run;
    }
    private void Update()
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
        if (popUp)
        {
            ShowPopUp(dmg);
        }
        
        if (life > 0)
        {
            StartCoroutine("DamageFeedback");
            life -= dmg;
            if (life <= 0)
            {
                Die();
            }
        }
        else if(life <= 0)
        {
            Die();
        }
        RefreshHealthbar();
    }

    void Die()
    {
        Drop();
        EnemySpawner.Get().spawnedEnemies.Remove(gameObject);
        anim.SetBool("die", true);
        
    }

    public void DeleteTomato()
    {
        Destroy(gameObject);
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
        Instantiate(soul, transform.position, Quaternion.identity);
    }

    void ShowPopUp(int dmg)
    {
        GameObject go = Instantiate(popUp, transform.position, Quaternion.identity, transform);
        go.GetComponent<TextMeshPro>().text = dmg.ToString();
    }
}
