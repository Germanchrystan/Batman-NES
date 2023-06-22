using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropClawPool : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    private int poolSize = 2;
    private List<GameObject> bulletList;

    private static DropClawPool instance;
    public static DropClawPool Instance { get { return instance; }}

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
    // Start is called before the first frame update
    void Start()
    {
        AddBulletsToPool(poolSize);
    }
    
    private void AddBulletsToPool(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bullet.DropClaw_Bullet.SetDirection(i % 2 == 0 ? - 1 : 1);
            bulletList.Add(bullet);
            bullet.transform.parent = transform;
        }
    }

    public GameObject RequestBullet()
    {
        for(int i = 0; i < bulletList.Count; i++)
        {
            if(!bulletList[i].activeSelf)
            {
                bulletList[i].SetActive(true);
                return bulletList[i];
            }
        }
        return null;
    }
}
