using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using System;

public class Tomato : Enemy
{
    Animator anim;
    Rigidbody rb;

    float distance = 0;

    enum EnemyAction
    {
        Idle,
        Run,
        Attack
    };
    EnemyAction actions;

    Player player;

    void Start()
    {
        player = Player.Get();
        enemyCollider = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        enemyAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        actions = EnemyAction.Run;
    }

    void Update()
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

    public void CheckIfCanAttack()
    {
        if (distance < attackDistance)
        {
            //Audio
            AkSoundEngine.PostEvent("Voz_enemigos", gameObject);
            AkSoundEngine.PostEvent("Ataque_enemigos", gameObject);
            player.TakeDamage(damage);
        }
        else
        {
            Chase();
        }
    }

    protected override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);

        if ((life <= 0) && !enemyCollider.isTrigger)
        {
            isAlive = false;
            enemyCollider.enabled = false;
            anim.SetBool("die", true);
        }
    }

    public void Chase()
    {
        actions = EnemyAction.Run;
    }

    protected override void Die()
    {
        //Audio
        AkSoundEngine.PostEvent("Muerte_enemigos", gameObject);
        Drop();
        Destroy(gameObject,timeToDie);
    }

    void Drop()
    {
        Instantiate(soul, spawnPos.transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            int randDmg = (int)UnityEngine.Random.Range(
                other.GetComponent<Weapon>().minDamage,
                other.GetComponent<Weapon>().maxDamage);
            TakeDamage(randDmg);
        }
    }
}
