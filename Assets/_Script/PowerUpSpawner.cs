using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ammoPrefab;
    [SerializeField] private GameObject heartPrefab;

    private const string poolSize = 9;
    string[] poolOrder = new string[] { "heart", "ammo", null };
    private List<GameObject> powerUpList = new List <GameObject>();
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AddPowerUpsToPool()
    {
        int powerUpIndex = 0;

        for(int i; i < poolSize; i++)
        {
            if(i > 0 && i % 3 == 0) powerUpIndex++;
            GameObject powerUp = InstantiatePowerUp(poolOrder[powerUpIndex]);
            powerUp.SetActive(false);
            bullet.transform.parent = transform;
        }
    }

    private GameObject InstantiatePowerUp(int powerUpIndex)
    {
        switch(powerUpIndex) {
            case "heart":
                GameObject powerUp = Instantiate(heartPrefab);
                return powerUp;
                break;
            case "ammo":
                GameObject powerUp = Instantiate(ammoPrefab);
                return powerUp;
                break;
            case default:
                return null;
        }
    }

    static public void RequestPowerUp(Transform gameObjectTransform)
    {
        Random r = new Random();
        int randomInt = r.Next(0, poolSize);
        if(powerUpList[randomInt] != null)
        {
            GameObject powerUpSelected = powerUpList[randomInt];
            powerUpSelected.SetActive(true);
            powerUpSelected.transform.parent = gameObjectTransform;
        }
    } 
}
