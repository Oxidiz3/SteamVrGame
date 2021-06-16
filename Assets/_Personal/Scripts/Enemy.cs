using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    private Transform _player;

    [NonSerialized] public AIManager AIManager;
    private NavMeshAgent _navMeshAgent;
    private void Die()
    {
        AIManager.EnemyDied();
        Destroy(gameObject);
    }

    private void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        _navMeshAgent.SetDestination(_player.position);
    }

    public void ApplyDamage()
    {
        health -= 34;
        if (health < 0)
        {
            Die();
        }
    }
}
