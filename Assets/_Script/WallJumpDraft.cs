// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class WallJumpDraft : MonoBehaviour
// {
// 	// Jump basic values
// 	public float speed=100f;
// 	bool jumpRequest;
// 	public float jumpForce=4f;
// 	public float fallMultiplier = 50f;
//     public float lowJumpMultiplier = 40f;

// 	// Jump Remember Times
// 	float JumpPressedRemember = 0;
//     [SerializeField]
//     float JumpPressedRememberTime = 0.1f;

//     float GroundedRemember = 0;
//     [SerializeField]
//     float GroundedRememberTime = 0.15f;
// 	private bool isTakingImpulse;

// 	// Grounded Check
// 	public Transform groundCheck;
// 	public LayerMask groundLayer;
// 	public float groundCheckRadius;
// 	public bool isGrounded;

// 	// General
// 	private Rigidbody2D rg;
// 	private Animator animator;
// 	public float movePause = .5f;

// 	// Movement
// 	private Vector2 movement;
// 	private bool facingRight = true;
// 	public float horizontalInput;
// 	private bool canMove = true;

// 	// Attack
// 	private bool isAttacking;

// 	// Wall-Jumping
// 	public Transform frontCheck;
// 	public Transform wallJumpAngle;
// 	public bool isFacingWall;
// 	public bool isWallSliding;
// 	public float frontCheckRadius;
// 	public float wallSlideSpeed = 3f;
// 	public Vector2 wallJumpDirection;
// 	public float wallJumpForce = 5;
// //=========================================================================================================//
// 	void Awake() 
// 	{
//     	rg=GetComponent<Rigidbody2D>();
// 	    animator=GetComponent<Animator>();
// 	}

// 	void Update() // Input Checking
// 	{
// 		if (!isAttacking && !isTakingImpulse && canMove)
// 		{

// 			//Se usa la función GetAxisRaw en vez de GetAxis porque Unity suele demorar un poco la devolución del Input.
// 			// GetAxisRaw devuelve los valores del Input de forma más inmediata.
// 			horizontalInput = Input.GetAxisRaw("Horizontal");
// 			movement = new Vector2(horizontalInput, 0f);

// 			//Girar al personaje
// 			if (horizontalInput < 0f && facingRight == true)
// 			{
// 				Flip();
// 			}
// 			else if (horizontalInput > 0f && facingRight == false)
// 			{
// 				Flip();
// 			}
// 		}

// 		CheckSurroundings();


// 		//Jump
// 		GroundedRemember -= Time.deltaTime;
//         if (isGrounded) 
// 		{
//             GroundedRemember = GroundedRememberTime;
//         }

//         JumpPressedRemember -= Time.deltaTime;
//         if (Input.GetButtonDown("Jump")) 
// 		{
//             JumpPressedRemember = JumpPressedRememberTime;
//         }

// 		if((JumpPressedRemember > 0) && !isAttacking==false) 
// 		{
// 			if(isGrounded)
// 			{
// 				JumpPressedRemember = JumpPressedRememberTime;
// 			} 
// 		}
		

// 		if ((JumpPressedRemember > 0) && (GroundedRemember > 0))
//         {
//             JumpPressedRemember = 0;
//             GroundedRemember = 0;
//             jumpRequest = true;
// 		}
		
// 		//Esto recibe el input del Ataque
// 		if(Input.GetButtonDown("Fire1") && isGrounded == true && isAttacking==false) 
// 		{
// 			movement = Vector2.zero;
// 			rg.velocity = Vector2.zero;
// 			animator.SetTrigger("Attack");
// 		}

// 		if(isWallSliding && Input.GetButtonDown("Jump")) 
// 		{
// 			WallJump();
// 		}

// 		// Wall Jump
// 		CheckWallSliding(horizontalInput);
// 	}

// 	void FixedUpdate() //Aquí se suele dar movimiento al personaje en base al Input
// 	{	
// 		// Running movement
// 		if (!isAttacking && !isTakingImpulse && canMove)
// 		{
// 			float horizontalVelocity = movement.normalized.x * speed;
// 			rg.velocity = new Vector2(horizontalVelocity, rg.velocity.y);
// 		}

// 		//Checking gravity
// 		if (rg.velocity.y < 0) 
// 		{
//             rg.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
//         } else if (rg.velocity.y > 0 && !Input.GetButton("Jump")) 
// 		{
//             rg.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
//         }

// 		if (jumpRequest)
// 		{	
// 			if(!isWallSliding) 
// 			{
// 				animator.SetTrigger("JumpImpulse");
// 				StartCoroutine("StopMove");
// 				isTakingImpulse=true;
// 				jumpRequest = false;
// 			}
// 		}

// 		if(isWallSliding)
// 		{
// 			if(rg.velocity.y < -wallSlideSpeed)
// 			{
// 				rg.velocity = new Vector2(rg.velocity.x, 2);
// 			}
// 		}
	
// 	}


// 	void LateUpdate()
// 	{
// 		animator.SetBool("Idle", movement == Vector2.zero); //Idle será true siempre que movement sea igual al vector (0,0) 
// 		animator.SetBool("isGrounded", isGrounded); //Booleano "isGrounded" en el script se conecta con el booleano homónimo del animator
// 		animator.SetFloat("JumpVelocity", rg.velocity.y); //Float "JumpVelocity" del animator se conecta con el float del velocity en Y
// 		animator.SetBool("IsWallSliding", isWallSliding);
		
// 		if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
// 		{
// 			isAttacking = true;
// 		} else
// 		{
// 			isAttacking = false;
// 		}
		
// 	}
// //=========================================================================================================//
// 	void Flip()
// 	{
// 		facingRight = !facingRight;//Con esto invertimos el valor del booleano. Si era true ahora es false y viceversa

// 		float localScaleX = transform.localScale.x; //Obtenemos la escala del personaje en X
// 		localScaleX = localScaleX * -1f; //Invertimos la escala en X
// 		transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z); //Se asigna el nuevo valor de X a la escala
// 	}

// 	private void CheckSurroundings()
// 	{
// 		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
// 		isFacingWall = Physics2D.OverlapCircle(frontCheck.position, frontCheckRadius, groundLayer);
// 	}

// 	public void GetJumpRequest() 
// 	{
// 		isTakingImpulse=false;
// 		rg.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse);//ForceMode2D.Impulse le da más impulso al salto
// 	}

// 	private void CheckWallSliding(float horizontalInput)
// 	{
// 		if(!isGrounded && isFacingWall && rg.velocity.y < 0 && horizontalInput != 0)
// 		{
// 			isWallSliding = true;

// 		} else 
// 		{
// 			isWallSliding = false;
// 		}
// 	}

// 	private void WallJump()
// 	{
// 		float facingDirection = facingRight ? 1f : -1f;
// 		Vector2 force = new Vector2(wallJumpDirection.x * wallJumpForce * -facingDirection, wallJumpDirection.y * wallJumpForce);
// 		rg.velocity = Vector2.zero;
// 		rg.AddForce(force, ForceMode2D.Impulse);
// 		Flip();
// 		StartCoroutine("StopMove");
// 	}


// 	IEnumerator StopMove()
// 	{
// 		canMove = false;
// 		// transform.localScale = transform.localScale.x == 1 ? new Vector2(-1,1): Vector2.one;
// 		// // transform.localScale = Vector2.one;
// 		if(isGrounded)
// 		{
// 			canMove = true;
// 		}
// 		yield return new WaitForSeconds(movePause);
// 		canMove = true;
// 	}
// }
