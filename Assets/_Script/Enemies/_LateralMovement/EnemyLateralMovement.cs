using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLateralMovement : MonoBehaviour
{
    private Rigidbody2D rg;
    [SerializeField] private int direction = -1;
    [SerializeField] private LateralSpeedSO lateralSpeedSO;
    

    void Awake()
    {
        rg = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        rg.velocity = new Vector2(direction * lateralSpeedSO.speed, rg.velocity.y);
    }
}
