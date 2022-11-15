using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    // General
	private Rigidbody2D rg;
	private Animator animator;
	public float movePause = .1f;

    // Movement
    public float speed=50f;
	private bool facingLeft = true;
    private int direction = -1;

    // Check
    public Transform frontCheck;
    public Transform groundCheck;
    private float frontCheckDistance = 5f;
    private float groundCheckRadius = 10f;
    Vector2 rayCastDirection;
    public bool frontGroundCheck;
    public LayerMask groundLayer;
    RaycastHit2D frontHitInfo; 
    public bool shouldFlip;


    void Awake()
    {
        rg=GetComponent<Rigidbody2D>();
        animator=GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckSurroundings();
     	//Girar al personaje
		if (shouldFlip)
		{
		    Flip();
		} 
    }

    void FixedUpdate() {
        rg.velocity = new Vector2(speed * direction, rg.velocity.y);
    }

    //=========================================================================================================//
	void Flip()
	{
		facingLeft = !facingLeft;//Con esto invertimos el valor del booleano. Si era true ahora es false y viceversa

		float localScaleX = transform.localScale.x; //Obtenemos la escala del personaje en X
		localScaleX = localScaleX * -1f; //Invertimos la escala en X
		transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z); //Se asigna el nuevo valor de X a la escala

        if (facingLeft) {
            direction = -1;
        } else {
            direction = 1;
        }
    }

    private void CheckSurroundings()
	{
		frontGroundCheck = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        rayCastDirection = facingLeft ? Vector2.left : Vector2.right;
        frontHitInfo = Physics2D.Raycast(frontCheck.position, rayCastDirection, frontCheckDistance, groundLayer);
        shouldFlip = !frontGroundCheck ||  frontHitInfo.collider != null;
    }
}
