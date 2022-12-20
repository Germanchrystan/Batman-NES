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
	// Jump basic values
	public float speed=100f;
	bool jumpRequest;
	public float jumpForce=4f;
	public float fallMultiplier = 50f;
    public float lowJumpMultiplier = 40f;

	// Jump Remember Times
	float JumpPressedRemember = 0;
    [SerializeField]
    float JumpPressedRememberTime = 0.1f;

    float GroundedRemember = 0;
    [SerializeField]
    float GroundedRememberTime = 0.01f;

	// Grounded Check
	public Transform groundCheck;
	public LayerMask groundLayer;
	public float groundCheckRadius;
	public bool isGrounded;

	// General
	private Rigidbody2D rg;
	public float movePause = .1f;

	// Movement
	private Vector2 movement;
	private bool facingRight = true;
	public float horizontalInput;
	private bool canMove = true;

	// Attack
	public Transform hitBox;
	
	private bool isAttacking;
	private bool isCrouching = false;
	private float verticalInput;

	// Fire
	private FireState currentFireState;
	private int ammo = 100;
	public GameObject batarang, pistolBullet, tripleBullet;
	
	// Animator
	private Animator animator;
	private string currentAnimationState;

	private bool bypassLateUpdate = false;

	const string RUNNING = "Running";
	const string IDLE = "Idle";
	const string JUMPING = "Jump";
	const string CROUCHING = "Crouching";

//=========================================================================================================//
	void Awake() 
	{
    	rg=GetComponent<Rigidbody2D>();
	    animator=GetComponent<Animator>();
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

		//Jump
		GroundedRemember -= Time.deltaTime;
        if (isGrounded) 
		{
            GroundedRemember = GroundedRememberTime;
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
		
		// Esto recibe el input del Ataque
		if(Input.GetButtonDown("Fire1") && isGrounded == true && isAttacking == false) 
		{
			StartCoroutine(AttackCoroutine());
		}

		if(Input.GetButtonDown("Fire2") && isGrounded == true && isAttacking == false)
		{
			StartCoroutine(FireCoroutine());
		}

		// Lateral movement
		if (!isAttacking && canMove)
		{

			//Se usa la función GetAxisRaw en vez de GetAxis porque Unity suele demorar un poco la devolución del Input.
			// GetAxisRaw devuelve los valores del Input de forma más inmediata.
			horizontalInput = Input.GetAxisRaw("Horizontal");
			movement = new Vector2(horizontalInput, 0f);

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
		if (!isAttacking && canMove && !isCrouching)
		{	
			float horizontalVelocity = movement.normalized.x * speed;
			rg.velocity = new Vector2(horizontalVelocity, rg.velocity.y);
		}

		//Checking gravity
		if (rg.velocity.y < 0) 
		{
            rg.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } 
		else if (rg.velocity.y > 0 && !Input.GetButton("Jump")) 
		{
            rg.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

		if (jumpRequest)
		{	
			jumpRequest = false;
		}
	
	}
//=========================================================================================================//
	void LateUpdate()
	{
		string newAnimationState = IDLE;
		if(!bypassLateUpdate)
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
		facingRight = !facingRight;//Con esto invertimos el valor del booleano. Si era true ahora es false y viceversa

		float localScaleX = transform.localScale.x; //Obtenemos la escala del personaje en X
		localScaleX = localScaleX * -1f; //Invertimos la escala en X
		transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z); //Se asigna el nuevo valor de X a la escala
	}

	private void CheckSurroundings()
	{
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
	}

	IEnumerator Jump() 
	{	
		bypassLateUpdate = false;
		yield return new WaitForSeconds(0.11f);
		isCrouching = false;
		rg.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse); //ForceMode2D.Impulse le da más impulso al salto
	}

	IEnumerator AttackCoroutine()
	{
		canMove = false; 
		rg.velocity = Vector2.zero;
		if(!isCrouching){
			animator.SetTrigger("Attack");
		} else {
			animator.SetTrigger("CrouchPunch");
		}
		yield return new WaitForSeconds(0.11f);
		canMove = true;
	}

	IEnumerator FireCoroutine()
	{
		canMove = false;
		rg.velocity = Vector2.zero;
		switch(currentFireState)
		{
			case FireState.BATARANG:
				if(ammo >= 1)
				{
					TriggerFireAnimation("FireBatarang", "CrouchFireBatarang");
					ammo -= 1;
				}
				break;
			case FireState.PISTOL:
				if(ammo >= 2)
				{
					TriggerFireAnimation("FirePistol", "CrouchFirePistol");
					ammo -= 2;
				}
				break;
			case FireState.TRIPLE:
				if(ammo >= 3)
				{
					TriggerFireAnimation("FireTriple", "CrouchFireTriple");
					ammo -= 3;
				}
    			break;
			default:
    			Debug.Log("Unknwon Fire State");
				break;
		}
		Debug.Log(ammo);
		yield return new WaitForSeconds(0.11f);
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

	void TriggerFireAnimation(string standingAnimation, string crouchingAnimation)
	{
		Debug.Log(standingAnimation);
		/*
		if(!isCrouching)
		{
			animator.SetTrigger(standingAnimation);
		} else {
			animator.SetTrigger(crouchingAnimation);
		}
		*/
	}

	public void BatarangSpawn()
	{
		GameObject instantiatedFire = (GameObject) Instantiate(batarang, hitBox.position, hitBox.rotation);
		
		Batarang batarangScript = instantiatedFire.GetComponent<Batarang>();
		batarangScript.direction = facingRight ? Vector2.right : Vector2.left;
		batarangScript.returnPoint = hitBox;
	}

	public void PistolBulletSpawn()
	{
		GameObject instantiatedFire = (GameObject) Instantiate(pistolBullet, hitBox.position, hitBox.rotation);

		PistolBullet pistolBulletScript = instantiatedFire.GetComponent<PistolBullet>();
		pistolBulletScript.direction = facingRight ? Vector2.right : Vector2.left;

	}

	public void TripleSpawn()
	{
		GameObject instantiatedFire = (GameObject) Instantiate(tripleBullet, hitBox.position, hitBox.rotation);

		TripleBullet tripleBulletScript = instantiatedFire.GetComponent<TripleBullet>();
		tripleBulletScript.direction = facingRight ? Vector2.right : Vector2.left;
	}

	void ChangeAnimationState(string newAnimationState)
	{
		if (currentAnimationState == newAnimationState) return;

		animator.Play(newAnimationState);

		currentAnimationState = newAnimationState;
	}
}
