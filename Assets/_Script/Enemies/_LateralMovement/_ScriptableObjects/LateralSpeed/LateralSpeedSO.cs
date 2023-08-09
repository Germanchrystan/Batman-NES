using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Should Lateral Speed SO", menuName = "Scriptable Objects/Lateral Movement/Lateral Speed")]
public class LateralSpeedSO : ScriptableObject
{
    public float speed;

    public LateralSpeedSO(float speed)
    {
        this.speed = speed;
    }
}
