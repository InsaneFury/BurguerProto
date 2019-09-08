using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float damage = 1;
    public NavMeshAgent enemyAgent;
    public float attackDistance = 2;
    public float life = 100f;
    public Image healthBar;

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
        if(life > 0)
        {
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

    public void Die()
    {
        Destroy(gameObject);
    }

    public void RefreshHealthbar()
    {
        if (healthBar.fillAmount != life)
        {
            healthBar.fillAmount = life / 100f;
        }
    }
}
