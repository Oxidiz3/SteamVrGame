using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Valve.VR.InteractionSystem;

public class AIManager : MonoBehaviour
{
    public List<Transform> spawns;
    public GameObject enemyPrefab;
    public float spawnInterval = 3f;

    private int _maxEnemies = 5;
    private int _numEnemies = 0;
    private float _timeOfNextSpawn = 0f;
    
    // Update is called once per frame
    void Update()
    {
        // If enough time has passed by spawn a character
        if (Time.time > _timeOfNextSpawn && _numEnemies < _maxEnemies)
        {
            _timeOfNextSpawn = Time.time + spawnInterval;
            Spawn();
        }
            
    }

    private void Spawn()
    {
        _numEnemies += 1;
        // Pick a random spawn
        var spawn = spawns[Random.Range(0, spawns.Count)];
        // Spawn at the random spawn
        var enemy = Instantiate(enemyPrefab, spawn);
        // Set a place to call our messages back to
        enemy.GetComponent<Enemy>().AIManager = this;
    }

    public void EnemyDied()
    {
        _numEnemies -= 1;
    }
}
