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

    private bool _isTriggered;
    public bool IsTriggered { get => _isTriggered; set => _isTriggered = value; }
    private bool  _isSpawnPointVisible;
    public bool IsSpawnPointVisible{ get => _isSpawnPointVisible; set => _isSpawnPointVisible = value; }

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
        canSpawn = IsTriggered && !IsSpawnPointVisible;
        if(canSpawn)
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("SpawnTrigger"))
        {
           IsTriggered = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("SpawnTrigger"))
        {
            IsTriggered = false;
        }
    }
}
