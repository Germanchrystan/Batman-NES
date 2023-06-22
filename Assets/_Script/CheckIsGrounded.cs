using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIsGrounded : MonoBehaviour
{
    private Transform groundCheck;
    private Transform wallCheck;
	private LayerMask groundLayer;
	public float groundCheckRadius = .15f;
	private bool isGrounded;
    private bool touchedWall;
    
    void Awake()
    {
        groundLayer = LayerMask.GetMask("Ground");
        groundCheck = transform.Find("GroundCheck");
        wallCheck = transform.Find("WallCheck");
    }
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if(wallCheck == null)
        {
            wallCheck = transform.Find("WallCheck");
        }
        else
        {
            touchedWall = Physics2D.OverlapCircle(wallCheck.position, groundCheckRadius, groundLayer);
        }
    }
    public bool GetIsGrounded()
    {
        return isGrounded;
    }

    public bool GetTouchedWall()
    {
        return touchedWall;
    }
}
