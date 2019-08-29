using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float damage = 1;
    public NavMeshAgent enemyAgent;
    Player player;
    private void Start()
    {
        player = Player.Get();
        enemyAgent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        enemyAgent.SetDestination(player.transform.position);
    }
}
