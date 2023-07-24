using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Enemy Prefab Spawn", menuName = "Scriptable Objects/Enemy Prefab Spawn")]

public class EnemyPrefabSpawnSO : ScriptableObject
{
    public GameObject Enemy;

    public EnemyPrefabSpawnSO(GameObject Enemy)
    {
        this.Enemy = Enemy;
    }
}
