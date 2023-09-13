using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New enemy health SO", menuName = "Scriptable Objects/Enemy Health/Health SO")]
public class EnemyHealthSO : ScriptableObject
{
    public int healthPoints;

    public EnemyHealthSO(int healthPoints)
    {
        this.healthPoints = healthPoints;
    }
}
