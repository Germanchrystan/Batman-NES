using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Enemy damage amount", menuName = "Scriptable Objects/Enemy Damage amount/Enemy Damage amount SO")]
public class EnemyDamageAmountSO : ScriptableObject
{
    public int damageAmount;
    public EnemyDamageAmountSO(int damageAmount)
    {
        this.damageAmount = damageAmount;
    }
}
