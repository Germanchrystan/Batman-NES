using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public class PlayerMovement:MonoBehaviour
{
	public float speed=100f;
	
	// Jump basic values
	bool jumpRequest;
	public float jumpForce=4f;
	public float fallMultiplier = 50f;
    public float lowJumpMultiplier = 40f;

	// Jump Remember Times
	float JumpPressedRemember = 0;
    [SerializeField]
    float JumpPressedRememberTime = 0.2f;

    float GroundedRemember = 0;
    [SerializeField]
    float GroundedRememberTime = 0.25f;

	// Grounded Check
	public Transform groundCheck;
	public LayerMask groundLayer;
	public float groundCheckRadius;
	public bool isGrounded;

	//Referencias
	private Rigidbody2D rg;
	private Animator animator;

	//Movimiento
	private Vector2 movement;
	private bool facingRight = true;

	//Ataque
	private bool isAttacking;

	void Awake() //Usualmente en esta función se definen variables
	{
    	rg=GetComponent<Rigidbody2D>();
	    animator=GetComponent<Animator>();
	}

	void Update()//Aquí se suele obtener el input del jugador
	{
		if (isAttacking == false)
		{

			//Se usa la función GetAxisRaw en vez de GetAxis porque Unity suele demorar un poco la devolución del Input.
			// GetAxisRaw devuelve los valores del Input de forma más inmediata.
			float horizontalInput = Input.GetAxisRaw("Horizontal");
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

		//Comprobamos si el personaje está sobre el suelo
		//Para eso se usa el método OverlapCircle, en la clase Physics2D, que crea circulos invisibles, en la posición de groundCheck,
		//con el radio igual a groundCheckRadius, y que detecta "Overlap" con cualquier objeto que pertenezca al groundLayer.
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

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
        
		//Salto
		if((JumpPressedRemember > 0) && (isGrounded == true) && isAttacking==false) 
		{
			JumpPressedRemember = JumpPressedRememberTime;
			
		}

		if ((JumpPressedRemember > 0) && (GroundedRemember > 0))
        {
            JumpPressedRemember = 0;
            GroundedRemember = 0;
            jumpRequest = true;
		}
		
		//Esto recibe el input del Ataque
		if(Input.GetButtonDown("Fire1") && isGrounded == true && isAttacking==false) 
		{
			movement = Vector2.zero;
			rg.velocity = Vector2.zero;
			animator.SetTrigger("Attack");
		}
	}

	void FixedUpdate() //Aquí se suele dar movimiento al personaje en base al Input
	{
		if (isAttacking==false)
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

		if (jumpRequest) 
		{
			rg.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse);//ForceMode2D.Impulse le da más impulso al salto
			jumpRequest = false;
		}

	}


	void LateUpdate()
	{
		animator.SetBool("Idle", movement == Vector2.zero); //Idle será true siempre que movement sea igual al vector (0,0) 
		animator.SetBool("isGrounded", isGrounded); //Booleano "isGrounded" en el script se conecta con el booleano homónimo del animator
		animator.SetFloat("JumpVelocity", rg.velocity.y); //Float "JumpVelocity" del animator se conecta con el float del velocity en Y
  
		if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
		{
			isAttacking = true;
		} else
		{
			isAttacking = false;
		}
	}

	void Flip()
	{
		facingRight = !facingRight;//Con esto invertimos el valor del booleano. Si era true ahora es false y viceversa

		float localScaleX = transform.localScale.x; //Obtenemos la escala del personaje en X
		localScaleX = localScaleX * -1f; //Invertimos la escala en X
		transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z); //Se asigna el nuevo valor de X a la escala
	}
}
