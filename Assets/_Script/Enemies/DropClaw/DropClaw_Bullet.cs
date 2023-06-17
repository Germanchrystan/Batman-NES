using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CheckIsGrounded))]
public class DropClaw_Bullet : MonoBehaviour
{
    private Rigidbody2D rg;
    // Checking grounded
    private bool isGrounded;
    private CheckIsGrounded checkIsGrounded;
    // Movement
    private int direction = 1;
    private float groundedTimer = 1f;
    /*[SerializeField]*/ private float speed = 150f;
    // Checking wall
    private GameObject wallCheck;
    private bool touchedWall = false;
    // State
    private const string DROPPING = "Dropping";
    private const string GROUNDED = "Grounded";
    private string currentState = DROPPING;
    void Awake()
    {
        checkIsGrounded = GetComponent<CheckIsGrounded>();
        rg = GetComponent<Rigidbody2D>();
        
        // Setting wall check based on the direction
        wallCheck= new GameObject("WallCheck");
        wallCheck.transform.parent = this.transform;
        wallCheck.transform.localPosition = new Vector2(6 * direction, 0);
    }
    void Update()
    {
        if(currentState == DROPPING)
        {
            isGrounded = checkIsGrounded.GetIsGrounded();
            if(isGrounded)
            {
                currentState = GROUNDED;
            }
        }
        if(currentState == GROUNDED)
        {
            groundedTimer -= Time.deltaTime;
            touchedWall = checkIsGrounded.GetTouchedWall();
            if(groundedTimer <= 0 || touchedWall)
            {
                Destroy(this.gameObject);
            }
        }
    }
    void FixedUpdate()
    {
        if(currentState == GROUNDED)
        {
            rg.velocity = new Vector2(direction * speed, rg.velocity.y);
        }
    }
    public void setDirection (int direction)
    {
        direction = direction;
    }
}
