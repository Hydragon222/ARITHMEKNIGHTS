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
        EnemybehaviorM1 enemy = Instantiate(enemyPrefab);
        enemy.SetPool(enemyPool);
        return enemy;
    }
    void Update()
    {
        if (Time.time > timeSinceLastSpawn)
        {
            enemyPool.Get();
            timeSinceLastSpawn = Time.time + timeBetweenSpawns;
        }
    }
}
