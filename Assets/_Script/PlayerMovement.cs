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
	private bool isTakingImpulse;

	// Grounded Check
	public Transform groundCheck;
	public LayerMask groundLayer;
	public float groundCheckRadius;
	public bool isGrounded;

	// General
	private Rigidbody2D rg;
	private Animator animator;
	public float movePause = .1f;

	// Movement
	private Vector2 movement;
	private bool facingRight = true;
	public float horizontalInput;
	private bool canMove = true;

	// Attack
	private bool isAttacking;

	private bool isCrouching = false;
	private float verticalInput;

	// Fire
	private FireState currentFireState;
	private int ammo = 100;


//=========================================================================================================//
	void Awake() 
	{
    	rg=GetComponent<Rigidbody2D>();
	    animator=GetComponent<Animator>();
	}

	void Start()
	{
		currentFireState = FireState.BATARANG;
	}

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
        if (Input.GetButtonDown("Jump")) 
		{
            JumpPressedRemember = JumpPressedRememberTime;
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
		if (!isAttacking && !isTakingImpulse && canMove)
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

	void FixedUpdate() //Aquí se suele dar movimiento al personaje en base al Input
	{	
		// Running movement
		if (!isAttacking && !isTakingImpulse && canMove && !isCrouching)
		{	
			float horizontalVelocity = movement.normalized.x * speed;
			rg.velocity = new Vector2(horizontalVelocity, rg.velocity.y);
		}

		//Checking gravity
		if (rg.velocity.y < 0) 
		{
            rg.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (rg.velocity.y > 0 && !Input.GetButton("Jump")) 
		{
            rg.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

		if (jumpRequest && !isTakingImpulse)
		{	
			animator.SetTrigger("JumpImpulse");
			isTakingImpulse=true;
			jumpRequest = false;
		}
	
	}

	void LateUpdate()
	{
		animator.SetBool("Idle", movement == Vector2.zero); //Idle será true siempre que movement sea igual al vector (0,0) 
		animator.SetBool("isGrounded", isGrounded); //Booleano "isGrounded" en el script se conecta con el booleano homónimo del animator
		animator.SetFloat("JumpVelocity", rg.velocity.y); //Float "JumpVelocity" del animator se conecta con el float del velocity en Y
		animator.SetBool("isCrouching", isCrouching);

		if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
		{
			isAttacking = true;
		} else
		{
			isAttacking = false;
		}
		if(animator.GetCurrentAnimatorStateInfo(0).IsName("JumpImpulse"))
		{
			isTakingImpulse=true;
		} else {
			isTakingImpulse=false;
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

	public void GetJumpRequest() 
	{
		rg.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse); //ForceMode2D.Impulse le da más impulso al salto
		isTakingImpulse=false;
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
		Debug.Log("Current Fire State: " + currentFireState);
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
}
