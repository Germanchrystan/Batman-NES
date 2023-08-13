using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Initial Direction SO", menuName = "Scriptable Objects/Lateral Movement/Initial Direction")]
public class InitialDirectionSO : ScriptableObject
{
    public int direction;
    public InitialDirectionSO(int direction)
    {
        this.direction = direction;
    }
}
