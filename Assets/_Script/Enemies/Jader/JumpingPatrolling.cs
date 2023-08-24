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
        // if(CanJump)
        // {
        //     Jump();
        //     CanJump = false;
        // }
    }
    public void Jump()
    {
        Debug.Log("JUMP");
        rg.AddForce(Vector2.up * 100, ForceMode2D.Impulse);
    }
}
