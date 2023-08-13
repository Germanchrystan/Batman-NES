using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Should Lateral Speed SO", menuName = "Scriptable Objects/Spawner/Intermittent Spawner Timer")]
public class IntermittentSpawnerTimerSO : ScriptableObject
{
    public float timer;
    public IntermittentSpawnerTimerSO(float timer)
    {
        this.timer = timer;
    }
}
