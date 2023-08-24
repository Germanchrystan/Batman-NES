using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPatrolling : MonoBehaviour
{
    //------------------------------------------//
	// Grounded Check
	//------------------------------------------//
    private Rigidbody2D rg;
    private Animator animator;
    public float jumpForce = 300f;
    
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
    void Update()
    {
    }
    public void Jump()
    {
        rg.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
