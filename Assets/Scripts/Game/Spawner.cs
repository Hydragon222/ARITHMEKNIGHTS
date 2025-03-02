using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float timeBetweenSpawns = 4f;
    private float timeSinceLastSpawn;

    [SerializeField] private EnemybehaviorM1 enemyPrefab;
    private IObjectPool<EnemybehaviorM1> enemyPool;

    private GameObject playerObject;

    [SerializeField] private int maxEnemies; // Limit in Inspector
    private List<EnemybehaviorM1> allEnemies = new List<EnemybehaviorM1>();


    void Awake()
    {
        playerObject = GameObject.FindWithTag("Player");
        if (playerObject == null)
        {
            Debug.LogError("Player not found! Make sure the Player GameObject has the 'Player' tag.");
        }
        enemyPool = new ObjectPool<EnemybehaviorM1>(CreateEnemy, OnGet, OnRelease);
    }

    private void OnGet(EnemybehaviorM1 enemy)
    {
        if (enemy == null) return;

        enemy.gameObject.SetActive(true);
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        enemy.transform.position = randomSpawnPoint.position;

        if (playerObject != null)
        {
            enemy.SetTarget(playerObject);
        }
    }

    private void OnRelease(EnemybehaviorM1 enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private EnemybehaviorM1 CreateEnemy()
    {
        if (allEnemies.Count >= maxEnemies)
        {
            return null; // Stop creating new instances
        }

        EnemybehaviorM1 enemy = Instantiate(enemyPrefab);
        enemy.SetPool(enemyPool);
        allEnemies.Add(enemy); // Keep track of created enemies
        return enemy;
    }
    void Update()
    {
        if (Time.time > timeSinceLastSpawn)
        {
        EnemybehaviorM1 enemy = enemyPool.Get();
                if (enemy != null) // Ensure we don't try to use null enemies
                {
                    timeSinceLastSpawn = Time.time + timeBetweenSpawns;
                }
        }
    }
}