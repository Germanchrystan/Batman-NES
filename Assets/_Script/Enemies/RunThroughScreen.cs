using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunThroughScreen : MonoBehaviour
{
    private Rigidbody2D rg;

    [SerializeField] private int direction = -1;
    private float speed = 150f;
    public RunThroughScreen(int direction)
    {
        this.direction = direction;
    }
    void Awake()
    {
        rg = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rg.velocity = new Vector2(direction * speed, rg.velocity.y);
    }
}
