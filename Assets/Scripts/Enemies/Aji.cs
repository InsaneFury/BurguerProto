using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using System;

public class Aji : Enemy
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

    [Header("Aji")]
    public GameObject explosion;
    public GameObject explotionParticle;

    void Start()
    {
        player = Player.Get();
        rb = GetComponent<Rigidbody>();
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

    public void Chase()
    {
        actions = EnemyAction.Run;
    }

    protected override void Attack()
    {
        Instantiate(explotionParticle, transform.position, Quaternion.identity);
        explosion.SetActive(true);
    }

    protected override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
        if ((life <= 0) && !enemyCollider.isTrigger)
        {
            enemyCollider.enabled = false;
            Die();
        }

    }

    protected override void Die()
    {
        //Audio
        AkSoundEngine.PostEvent("Muerte_enemigos", gameObject);
        Drop();
        Destroy(gameObject);
    }

    void Drop()
    {
        Instantiate(soul, spawnPos.transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().TakeDamage(damage);
        }

        if (other.CompareTag("Weapon"))
        {
            int randDmg = (int)UnityEngine.Random.Range(
                other.GetComponent<Weapon>().minDamage,
                other.GetComponent<Weapon>().maxDamage);
            TakeDamage(randDmg);
        }
    }
}
