using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPatrolling : MonoBehaviour
{
    //------------------------------------------//
	// Grounded Check
	//------------------------------------------//
    public Transform groundCheck;
	public LayerMask groundLayer;
	public float groundCheckRadius;

    private Rigidbody2D rg;
    private Animator animator;
    
    private int _jumpDirection = 0;
	public int JumpDirection { get => _jumpDirection; set => _jumpDirection = value; }
    
    private bool _canJump;
    public bool CanJump { get => _canJump; set => _canJump = value; }

    private Vector2 jumpVector;
    void Awake() 
	{
    	rg=GetComponent<Rigidbody2D>();
	    animator=GetComponent<Animator>();
	}

    // Update is called once per frame
    void Update()
    {
        if(CanJump)
        {

        }
    }

    void Jump()
    {
        rg.AddForce(Vector2.up, ForceMode2D.Impulse);
    }
}
