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
    private int _spawnPointDirection = 1;

    private Transform playerTransform;

    void Awake()
    {
        spawnerPoint = gameObject.transform.Find("SpawnerPoint").gameObject;
        enemyPrefabPool = GetComponent<EnemyPrefabPool>();
    }
    void Start()
    {
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
                // CheckSpawnPointDirection();
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
    void CheckSpawnPointDirection() // TODO: Use this function to set spawn position position
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        float positionSubtraction = transform.position.x - playerTransform.position.x;
        _spawnPointDirection = positionSubtraction > 0 ? 1 : -1;
    }
}
