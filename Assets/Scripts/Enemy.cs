using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float damage = 1;
    public NavMeshAgent enemyAgent;
    public float attackDistance = 2;

    Player player;
    Animator anim;

    enum EnemyAction : short {Idle = 0,Run,Attack};
    EnemyAction actions;

    private void Start()
    {
        player = Player.Get();
        enemyAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        actions = EnemyAction.Run;
    }
    private void Update()
    {
        switch (actions)
        {
            case EnemyAction.Idle:
                break;
            case EnemyAction.Run:
                enemyAgent.SetDestination(player.transform.position);
                if (enemyAgent.remainingDistance < attackDistance)
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
        //if (enemyAgent.remainingDistance < attackDistance)
        //{
        //    player.TakeDamage(damage);
        //}
        //else
        //{
            Chase();
        //}
    }
}
