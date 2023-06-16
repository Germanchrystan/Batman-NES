using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    LineRenderer line;
    [SerializeField] private Transform startPoint, endPoint; 

    void Awake()
    {
        line = GetComponent<LineRenderer>();
    }
    
    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, startPoint.position);
        line.SetPosition(1, endPoint.position);
    }
}
