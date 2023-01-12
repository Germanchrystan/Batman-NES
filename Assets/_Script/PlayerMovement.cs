using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

// Fire
public enum FireState
{
	BATARANG,
	PISTOL,
	TRIPLE
}

public class PlayerMovement:MonoBehaviour
{
	//------------------------------------------//
	// Jump basic values
	//------------------------------------------//
	public float speed=100f;
	bool jumpRequest;
	public float jumpForce=4f;
	public float fallMultiplier = 50f;
    public float lowJumpMultiplier = 40f;

	//------------------------------------------//
	// Jump Remember Times
	//------------------------------------------//
	float JumpPressedRemember = 0;
    [SerializeField]
    float JumpPressedRememberTime = 0.1f;

    float GroundedRemember = 0;
    [SerializeField]
    float GroundedRememberTime = 0.01f;

	//------------------------------------------//
	// Wall Jump
	//------------------------------------------//
	public Transform frontPoint;
	public Transform wallJumpPoint;
	private Vector2 wallJumpDirection;
	bool isFacingWall;
	bool canWallJump;
	public float frontCheckRadius = 4f;
	bool isWallJumpPressed = false;
	private bool isTakingWallJumpImpulse = false;
	private bool isWallJumping = false;

	//------------------------------------------//
	// Grounded Check
	//------------------------------------------//
	public Transform groundCheck;
	public LayerMask groundLayer;
	public float groundCheckRadius;
	public bool isGrounded;
	
	//------------------------------------------//
	// General
	//------------------------------------------//
	private Rigidbody2D rg;
	public float movePause = .1f;
	public PlayerHealth playerHealth;

	//------------------------------------------//
	// Movement
	//------------------------------------------//
	private Vector2 movement;
	private bool facingRight = true;
	public float horizontalInput;
	private bool canMove = true;

	//------------------------------------------//
	// Attack
	//------------------------------------------//
	public Transform hitBox;
	public Transform crouchHitBox;
	private bool isAttackPressed;
	private bool isAttacking;
	private float attackDelay = 0.25f;	
	
	//------------------------------------------//
	// Crouch
	//------------------------------------------//
	private bool isCrouching = false;
	private float verticalInput;

	//------------------------------------------//
	// Fire
	//------------------------------------------//
	private FireState currentFireState;
	private int ammo = 100;
	public GameObject batarang, pistolBullet, tripleBullet;
	private bool isFirePressed;
	private bool isFiring;
	
	//------------------------------------------//
	// Animator
	//------------------------------------------//
	private Animator animator;
	private string currentAnimationState;
	private bool bypassLateUpdate = false;

	//------------------------------------------//
	// Animations
	//------------------------------------------//
	const string RUNNING = "Running";
	const string IDLE = "Idle";
	const string JUMPING = "Jump";
	const string CROUCHING = "Crouching";
	const string DEATH = "Death";
	
	const string PUNCH = "Punch";
	const string CROUCH_PUNCH = "CrouchPunch";
	const string JUMP_PUNCH = "JumpPunch";

	const string FIRE_BATARANG = "FireBatarang";
	const string CROUCH_FIRE_BATARANG = "CrouchFireBatarang";
	const string JUMP_FIRE_BATARANG = "JumpFireBatarang";

	const string FIRE_PISTOL = "FirePistol";
	const string CROUCH_FIRE_PISTOL = "CrouchFirePistol";
	const string JUMP_FIRE_PISTOL = "JumpFirePistol";

	const string FIRE_TRIPLE = "FireTriple";
	const string CROUCH_FIRE_TRIPLE = "CrouchFireTriple";
	const string JUMP_FIRE_TRIPLE = "JumpFireTriple";

//=========================================================================================================//
	void Awake() 
	{
    	rg=GetComponent<Rigidbody2D>();
	    animator=GetComponent<Animator>();
		playerHealth=GetComponent<PlayerHealth>();
	}
//=========================================================================================================//
	void Start()
	{
		currentFireState = FireState.BATARANG;
	}
//=========================================================================================================//
	void Update() // Input Checking
	{

		CheckSurroundings();

		//------------------------------------------//
		//Jump
		//------------------------------------------//
		GroundedRemember -= Time.deltaTime;
        if (isGrounded) 
		{
            GroundedRemember = GroundedRememberTime;

			// Stop wall jumping if the character touches ground
			if(isWallJumping)
			{
				WallJumpComplete();
			}
        }

        JumpPressedRemember -= Time.deltaTime;
        if (Input.GetButtonDown("Jump") && isGrounded) 
		{
            JumpPressedRemember = JumpPressedRememberTime;
			bypassLateUpdate = true;
			StartCoroutine(Jump());
        }
		if((JumpPressedRemember > 0) && !isAttacking==false) 
		{
			if(isGrounded)
			{
				JumpPressedRemember = JumpPressedRememberTime;
			} 
		}
		

		if ((JumpPressedRemember > 0) && (GroundedRemember > 0))
        {
            JumpPressedRemember = 0;
            GroundedRemember = 0;
            jumpRequest = true;
		}

		//------------------------------------------//
		// Wall Jump
		//------------------------------------------//
		if(isFacingWall && !isGrounded)
		{
			if(Input.GetButtonDown("Jump"))
			{
				isWallJumpPressed = true;
			}
		}

		//------------------------------------------//
		// Attack
		//------------------------------------------//
		if(Input.GetButtonDown("Fire1") && !isAttacking && !isFiring) 
		{
			isAttackPressed = true;
		}

		//------------------------------------------//
		// Fire
		//------------------------------------------//
		if(Input.GetButtonDown("Fire2") && !isAttacking && !isFiring)
		{
			isFirePressed = true;
		}

		// Lateral movement
		if (!isAttacking && canMove && !isWallJumping)
		{

			// Se usa la función GetAxisRaw en vez de GetAxis porque Unity suele demorar un poco la devolución del Input.
			// GetAxisRaw devuelve los valores del Input de forma más inmediata.
			horizontalInput = Input.GetAxisRaw("Horizontal");
			movement = new Vector2(horizontalInput, 0f);

			if(playerHealth.currentHealth > 0)
			{
				//Girar al personaje
				if (horizontalInput < 0f && facingRight == true)
				{
					Flip();
				}
				else if (horizontalInput > 0f && facingRight == false)
				{
					Flip();
				}
			}
		}
		verticalInput = Input.GetAxisRaw("Vertical");
		if(verticalInput >= 0)
		{
			isCrouching = false;
		}
		if(isGrounded && rg.velocity.y == 0 && verticalInput < 0 )
		{
			isCrouching = true;
		}
		// Change Fire Weapon
		if (Input.GetButtonDown("Fire3"))
		{
			switchWeapon();
		}
	}
//=========================================================================================================//
	void FixedUpdate() //Aquí se suele dar movimiento al personaje en base al Input
	{	
		// Running movement
		if (!isAttacking && !isWallJumping && canMove && !isCrouching && playerHealth.currentHealth > 0)
		{	
			float horizontalVelocity = movement.normalized.x * speed;
			rg.velocity = new Vector2(horizontalVelocity, rg.velocity.y);
		}

		//Checking gravity
		if (rg.velocity.y < 0) 
		{
            rg.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } 
		else if ((rg.velocity.y > 0 && !Input.GetButton("Jump")) || (isWallJumping && !Input.GetButton("Jump"))) 
		{
            rg.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

		if (jumpRequest)
		{	
			jumpRequest = false;
		}

		if(isAttackPressed)
		{
			isAttackPressed = false;

			if(!isAttacking)
			{
				isAttacking = true;
				StartCoroutine(AttackCoroutine());
			}
		}

		if(isFirePressed)
		{
			isFirePressed = false;

			if(!isFiring && !isAttacking)
			{
				isFiring = true;
				StartCoroutine(FireCoroutine());
			}
		}

		if(isWallJumpPressed)
		{
			isWallJumpPressed = false;
			if(!isFiring && !isAttacking)
			{
				isTakingWallJumpImpulse = true;
				StartCoroutine(WallJumpCorroutine());
			}
		}

		if(isTakingWallJumpImpulse)
		{
			rg.gravityScale = 0;
			rg.velocity = Vector2.zero;
		}
		else
		{
			rg.gravityScale = 25;
		}
	}
//=========================================================================================================//
	void LateUpdate()
	{
		string newAnimationState = IDLE;
		if (playerHealth.currentHealth == 0)
		{
			newAnimationState = DEATH;
			ChangeAnimationState(newAnimationState);
		}
		else if(!bypassLateUpdate && !isAttacking && !isFiring && !isTakingWallJumpImpulse)
		{

			if(isGrounded)
			{
				if (movement != Vector2.zero)
				{
					newAnimationState = RUNNING;
				}
				else if(isCrouching)
				{
					newAnimationState = CROUCHING;
				}
			}
			else 
			{
				newAnimationState = JUMPING;
			} 
			ChangeAnimationState(newAnimationState);
		}
	}
//=========================================================================================================//
	void Flip()
	{
		facingRight = !facingRight; // Con esto invertimos el valor del booleano. Si era true ahora es false y viceversa

		float localScaleX = transform.localScale.x; // Obtenemos la escala del personaje en X
		localScaleX = localScaleX * -1f; // Invertimos la escala en X
		transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z); // Se asigna el nuevo valor de X a la escala
	}

	private void CheckSurroundings()
	{
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
		isFacingWall = Physics2D.OverlapCircle(frontPoint.position, frontCheckRadius, groundLayer);
	}

	IEnumerator Jump() 
	{	
		bypassLateUpdate = false;
		isCrouching = true;
		yield return new WaitForSeconds(0.11f);
		isCrouching = false;
		rg.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse); // ForceMode2D.Impulse le da más impulso al salto
	}

	IEnumerator WallJumpCorroutine()
	{
		ChangeAnimationState("WallJumpImpulse");
		canMove = false;
		yield return new WaitForSeconds(0.25f);
		Flip();
		wallJumpDirection = new Vector2(wallJumpPoint.position.x - transform.position.x, wallJumpPoint.position.y - transform.position.y);
		rg.AddForce(wallJumpDirection.normalized * jumpForce, ForceMode2D.Impulse);
		isWallJumping = true;
		canMove = true;
		WallJumpImpulseComplete();
	}

	IEnumerator AttackCoroutine()
	{
		if(isCrouching)
		{
			canMove = false; 
			rg.velocity = Vector2.zero;
			ChangeAnimationState(CROUCH_PUNCH); 
		}
		else if (!isGrounded)
		{
			ChangeAnimationState(JUMP_PUNCH);
		}
		else 
		{
			canMove = false; 
			rg.velocity = Vector2.zero;
			ChangeAnimationState(PUNCH);
		}
		yield return new WaitForSeconds(attackDelay);
		canMove = true;
		AttackComplete();
	}

	IEnumerator FireCoroutine()
	{
		switch(currentFireState)
		{
			case FireState.BATARANG:
				if(ammo >= 1)
				{
					TriggerFireAnimation(FIRE_BATARANG, CROUCH_FIRE_BATARANG, JUMP_FIRE_BATARANG);
					ammo -= 1;
				}
				break;
			case FireState.PISTOL:
				if(ammo >= 2)
				{
					TriggerFireAnimation(FIRE_PISTOL, CROUCH_FIRE_PISTOL, JUMP_FIRE_PISTOL);
					ammo -= 2;
				}
				break;
			case FireState.TRIPLE:
				if(ammo >= 3)
				{
					TriggerFireAnimation(FIRE_TRIPLE, CROUCH_FIRE_TRIPLE, JUMP_FIRE_TRIPLE);
					ammo -= 3;
				}
    			break;
			default:
    			Debug.Log("Unknwon Fire State");
				break;
		}
		yield return new WaitForSeconds(attackDelay);
		FireComplete();
		canMove = true;
	}

	void switchWeapon()
	{
		switch(currentFireState)
		{
			case FireState.BATARANG:
				currentFireState = FireState.PISTOL;
				break;
			case FireState.PISTOL:
				currentFireState = FireState.TRIPLE;
				break;
			case FireState.TRIPLE:
				currentFireState = FireState.BATARANG;
    			break;
			default:
    			Debug.Log("Unknwon Fire State");
				break;
		}
	}

	void TriggerFireAnimation(string standingAnimation, string crouchingAnimation, string jumpingAnimation)
	{
		if(isCrouching)
		{
			canMove = false; 
			rg.velocity = Vector2.zero;
			ChangeAnimationState(crouchingAnimation); 
		}
		else if (!isGrounded)
		{
			ChangeAnimationState(jumpingAnimation);
		}
		else 
		{
			canMove = false; 
			rg.velocity = Vector2.zero;
			ChangeAnimationState(standingAnimation);
		}
		
	}

	public void BatarangSpawn()
	{
		Transform hitPoint = isCrouching ? crouchHitBox : hitBox;
		GameObject instantiatedFire = (GameObject) Instantiate(batarang, hitPoint.position, hitPoint.rotation);
		
		Batarang batarangScript = instantiatedFire.GetComponent<Batarang>();
		batarangScript.direction = facingRight ? Vector2.right : Vector2.left;
		batarangScript.returnPoint = gameObject.transform;
	}

	public void PistolBulletSpawn()
	{
		Transform hitPoint = isCrouching ? crouchHitBox : hitBox;
		GameObject instantiatedFire = (GameObject) Instantiate(pistolBullet, hitPoint.position, hitPoint.rotation);

		PistolBullet pistolBulletScript = instantiatedFire.GetComponent<PistolBullet>();
		pistolBulletScript.direction = facingRight ? Vector2.right : Vector2.left;

	}

	public void TripleSpawn()
	{
		Transform hitPoint = isCrouching ? crouchHitBox : hitBox;
		GameObject instantiatedFire = (GameObject) Instantiate(tripleBullet, hitPoint.position, hitPoint.rotation);

		TripleBullet tripleBulletScript = instantiatedFire.GetComponent<TripleBullet>();
		tripleBulletScript.direction = facingRight ? Vector2.right : Vector2.left;
	}

	void ChangeAnimationState(string newAnimationState)
	{
		if (currentAnimationState == newAnimationState) return;

		animator.Play(newAnimationState);

		currentAnimationState = newAnimationState;
	}
	
	void AttackComplete()
	{
		isAttacking = false;
	}

	void FireComplete()
	{
		isFiring = false;
	}

	void WallJumpImpulseComplete()
	{
		isTakingWallJumpImpulse = false;
	}

	void WallJumpComplete()
	{
		isWallJumping = false;
	}
}
