using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Timer duration SO", menuName = "Scriptable Objects/Timer/Timer duration SO")]
public class TimerDurationSO : ScriptableObject
{
    public float time;

    public TimerDurationSO(float time)
    {
        this.time = time;
    }
}
