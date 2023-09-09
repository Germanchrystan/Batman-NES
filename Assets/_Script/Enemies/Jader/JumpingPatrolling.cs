using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class JumpingPatrolling : MonoBehaviour
{
    //------------------------------------------//
	// Grounded Check
	//------------------------------------------//
    private Rigidbody2D rg;
    private Animator animator;
    public float jumpForce = 300f;

    private Vector2 jumpVector; 
    private GameObject FoundPlayer;
    private int xDirection;
    private Vector2 jumpDirection;

    private int _jumpDirection = 0;
	public int JumpDirection { get => _jumpDirection; set => _jumpDirection = value; }
    private bool _canJump;
    public bool CanJump { get => _canJump; set => _canJump = value; }
    private int _facingDirection = -1;
    public int FacingDirection { get => _facingDirection; set => _facingDirection = value; }
    public UnityEvent FlipEvent;

    void Awake() 
	{
    	rg=GetComponent<Rigidbody2D>();
	    animator=GetComponent<Animator>();
	}
    public void SetFoundPlayer(GameObject found)
    {
        this.FoundPlayer = found;
    }
    private void CalculateJumpDirection ()
    {
        float playerXPosition = FoundPlayer.transform.position.x;
        xDirection = (playerXPosition - gameObject.transform.position.x) > 0 ? 1 : -1;
        if(FacingDirection != xDirection) 
        {
            FlipEvent.Invoke();
            ChangeFacingDirection();
        }
    }
    public void Jump()
    {   
        if(FoundPlayer != null)
        {
            CalculateJumpDirection();
        }
        jumpDirection = new Vector2(xDirection, 1).normalized;
        rg.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
    }
    public void ChangeFacingDirection ()
    {
        FacingDirection *= -1;
    }
}
