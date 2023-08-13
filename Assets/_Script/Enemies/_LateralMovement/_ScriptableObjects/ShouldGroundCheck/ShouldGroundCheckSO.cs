using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Should Ground Check SO", menuName = "Scriptable Objects/Lateral Movement/Should Ground Check")]
public class ShouldGroundCheckSO : ScriptableObject
{
    public bool shouldGroundCheck;

    public ShouldGroundCheckSO(bool shouldGroundCheck)
    {
        this.shouldGroundCheck = shouldGroundCheck;
    }
}
