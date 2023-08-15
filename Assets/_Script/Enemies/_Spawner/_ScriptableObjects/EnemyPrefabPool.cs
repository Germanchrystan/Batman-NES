using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPrefabPool : MonoBehaviour
{   
    // Scriptable Object variables
    [SerializeField] private EnemyPrefabSpawnSO enemyPrefabSpawnSO;
    private GameObject enemyPrefab;
    private int poolSize;
    // Pool
    private List<GameObject> poolList = new List<GameObject>(); 
    private int currentPoolIndex = 0;

    public Transform spawnerPointTransform;

    void Start()
    {
        if(enemyPrefabSpawnSO != null)
        {
            enemyPrefab = enemyPrefabSpawnSO.Enemy;
            poolSize = enemyPrefabSpawnSO.poolSize;
        }
        spawnerPointTransform = gameObject.transform.Find("SpawnerPoint").gameObject.transform;
        AddInstancesToPool(poolSize);
    }
    public void AddInstancesToPool(int poolSize)
    {
        for(int i = 0; i < poolSize; i++)
        {
            GameObject prefabInstance = Instantiate(enemyPrefab);
            prefabInstance.SetActive(false);
            poolList.Add(prefabInstance);
            prefabInstance.transform.parent = spawnerPointTransform;
            prefabInstance.transform.localPosition = new Vector3(0, 0, 0);
        }
    }
    public GameObject RequestPrefabInstance()
    {
        if(!poolList[currentPoolIndex].activeSelf)
        {
            GameObject returnedInstance = poolList[currentPoolIndex];
            IncreasePoolIndex();
            returnedInstance.transform.localPosition = new Vector3(0, 0, 0);
            returnedInstance.SetActive(true);
            return returnedInstance;
        }
        // IncreasePoolIndex();
        return null;
    }
    private void IncreasePoolIndex()
    {
        currentPoolIndex = currentPoolIndex + 1 == poolSize ? 0 : currentPoolIndex + 1; 
    }
}
