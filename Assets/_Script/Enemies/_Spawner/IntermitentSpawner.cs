using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntermitentSpawner : MonoBehaviour
{
    private GameObject spawnerPoint;
    public EnemyPrefabPool enemyPrefabPool;
    [SerializeField] IntermittentSpawnerTimerSO intermittentSpawnerTimerSO;
    private float timeBetweenSpawns;
    private float currentTimer;

    private bool canSpawn;

    void Awake()
    {
        spawnerPoint = gameObject.transform.Find("SpawnerPoint").gameObject;
    }
    void Start()
    {
        enemyPrefabPool = EnemyPrefabPool.Instance;
        timeBetweenSpawns = intermittentSpawnerTimerSO.timer;
        currentTimer = timeBetweenSpawns;
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
