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
    float distance = 0;

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
        distance = Vector3.Distance(transform.position, player.transform.position);
        switch (actions)
        {
            case EnemyAction.Idle:
                break;
            case EnemyAction.Run:
                enemyAgent.SetDestination(player.transform.position);

                
                if (distance < attackDistance)
                {
                    Debug.Log("enemyAgent.remainingDistance: " + enemyAgent.remainingDistance);
                    Debug.Log("attackDistance: " + attackDistance);
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
}
