using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New PowerUp Method", menuName = "Scriptable Objects/Power Up Method")]
public class PowerUpType : ScriptableObject
{
    public string powerUpMethod;

    public PowerUpType(string powerUpMethod)
    {
        this.powerUpMethod = powerUpMethod;
    }
}
