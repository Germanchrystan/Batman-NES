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
    [SerializeField] private GameObject _foundPlayer;
    public GameObject FoundPlayer{ get => _foundPlayer; set => _foundPlayer = value; }
    void Awake() 
	{
    	rg=GetComponent<Rigidbody2D>();
	    animator=GetComponent<Animator>();
	}
    void Update() {}
    public void SetFoundPlayer(GameObject found)
    {
        FoundPlayer = found;
    }

    private void CalculateJumpDirection ()
    {
        float playerXPosition = FoundPlayer.transform.position.x;
        Debug.Log(playerXPosition);
    }
    public void Jump()
    {   
        Debug.Log(FoundPlayer);
        // CalculateJumpDirection();
        rg.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
