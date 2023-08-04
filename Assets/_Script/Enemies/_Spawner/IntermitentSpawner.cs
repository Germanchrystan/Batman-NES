using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntermitentSpawner : MonoBehaviour
{
    private GameObject spawnerPoint;
    public EnemyPrefabPool enemyPrefabPool;
    private float timeBetweenSpawns = 1f;
    private float currentTimer;

    void Awake()
    {
        spawnerPoint = gameObject.transform.Find("SpawnerPoint").gameObject;
        currentTimer = timeBetweenSpawns;
    }
    void Start()
    {
        enemyPrefabPool = EnemyPrefabPool.Instance;
    }
    void Update()
    {
        if(spawnerPoint.activeSelf)
        {
            currentTimer -= Time.deltaTime;
            if(currentTimer <= 0f)
            {
                currentTimer = timeBetweenSpawns;
                Spawn();
            }
        }
    }
    private void Spawn()
    {
        GameObject enemyInstance = enemyPrefabPool.RequestPrefabInstance();
    }
}
