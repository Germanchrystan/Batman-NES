using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropClawPool : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    private int poolSize = 2;
    private int currentPoolBullet = 0;
    private List<GameObject> bulletList = new List <GameObject>();

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
            bulletList.Add(bullet);
            bullet.transform.parent = transform;
            // Setting direction for bullet
            int bulletDirection = i % 2 == 0 ? -1 : 1;
            DropClaw_Bullet dropClawBulletScript = bullet.GetComponent<DropClaw_Bullet>();
            if(dropClawBulletScript != null) dropClawBulletScript.SetDirection(bulletDirection);
        }
    }

    public GameObject RequestBullet(int currentPoolBullet)
    {
        if(!bulletList[currentPoolBullet].activeSelf)
        {
            bulletList[currentPoolBullet].SetActive(true);
            return bulletList[currentPoolBullet];
        }
        return null;
    }
}
