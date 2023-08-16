using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{    
    public void Death()
    {
        //PowerUpSpawner.RequestPowerUp(gameObject.transform);
        gameObject.SetActive(false);
    }
}
