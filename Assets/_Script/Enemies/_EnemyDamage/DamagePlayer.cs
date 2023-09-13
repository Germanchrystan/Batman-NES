using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    private int damageAmount = 1;
    [SerializeField] EnemyDamageAmountSO enemyDamageAmountSO;

    void Start()
    {
        if(enemyDamageAmountSO != null)
        {
            damageAmount = enemyDamageAmountSO.damageAmount;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.SendMessageUpwards("GetDamage", damageAmount);
        }
    }
}
