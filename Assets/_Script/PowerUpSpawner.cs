using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ammoPrefab;
    [SerializeField] private GameObject heartPrefab;

    private const int poolSize = 9;
    string[] poolOrder = new string[] { "heart", "ammo", null };
    private static List<GameObject> powerUpList = new List <GameObject>();
    private static PowerUpSpawner instance;
    public static PowerUpSpawner Instance { get { return instance; }}

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        AddPowerUpsToPool();
    }

    private void AddPowerUpsToPool()
    {
        int powerUpIndex = 0;

        for(int i = 0; i < poolSize; i++)
        {
            if(i > 0 && i % 3 == 0) powerUpIndex++;
            GameObject powerUp = InstantiatePowerUp(poolOrder[powerUpIndex]);
            powerUp.SetActive(false);
            powerUpList.Add(powerUp);
            powerUp.transform.parent = transform;
        }
    }

    private GameObject InstantiatePowerUp(string powerUpType)
    {
        switch(powerUpType) {
            case "heart":
                return Instantiate(heartPrefab);
            case "ammo":
                return Instantiate(ammoPrefab);
            default:
                GameObject none = new GameObject("null");
                return none;
        }
    }

    static public void RequestPowerUp(Transform gameObjectTransform)
    {
        int randomInt = Random.Range(0, poolSize);
        if(powerUpList[randomInt].name != "null")
        {
            GameObject powerUpSelected = powerUpList[randomInt];
            powerUpSelected.SetActive(true);
            powerUpSelected.transform.position = gameObjectTransform.position;
        }
    }
    static public void DestroyPowerUp(GameObject powerUp)
    {
        powerUp.SetActive(false);
        // powerUp.transform.parent = transform;
    }
}
