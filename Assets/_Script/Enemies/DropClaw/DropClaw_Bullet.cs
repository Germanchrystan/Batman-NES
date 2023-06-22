using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CheckIsGrounded))]
[RequireComponent(typeof(AnimationStateChanger))]
public class DropClaw_Bullet : MonoBehaviour
{
    private Rigidbody2D rg;
    // Checking grounded
    private bool isGrounded;
    private CheckIsGrounded checkIsGrounded;
    // Movement
    private int direction = 1;
    private float groundedTimer = 1f;
    /*[SerializeField]*/ private float speed = 150f;
    // Checking wall
    private GameObject wallCheck;
    private bool touchedWall = false;
    // State
    private const string DROPPING = "Dropping";
    private const string GROUNDED = "Grounded";
    private const string EXPLODE = "Explode";
    private string currentState = DROPPING;
    // Animation
	private Animator animator;
    private AnimationStateChanger animationStateChanger;
    void Awake()
    {
        checkIsGrounded = GetComponent<CheckIsGrounded>();
        rg = GetComponent<Rigidbody2D>();
        animator=GetComponent<Animator>();
        animationStateChanger=GetComponent<AnimationStateChanger>();
        
        // Setting wall check based on the direction
        wallCheck= new GameObject("WallCheck");
        wallCheck.transform.parent = this.transform;
        wallCheck.transform.localPosition = new Vector2(10 * direction, 0);
    }
    void Update()
    {
        if(currentState == DROPPING)
        {
            isGrounded = checkIsGrounded.GetIsGrounded();
            if(isGrounded)
            {
                currentState = GROUNDED;
            }
        }
        if(currentState == GROUNDED)
        {
            groundedTimer -= Time.deltaTime;
            touchedWall = checkIsGrounded.GetTouchedWall();
            if(groundedTimer <= 0 || touchedWall)
            {
                currentState = animationStateChanger.ChangeAnimationState(animator, currentState, EXPLODE);
            }
        }
    }
    void FixedUpdate()
    {
        if(currentState == GROUNDED)
        {
            rg.velocity = new Vector2(direction * speed, rg.velocity.y);
        }
    }
    public void SetDirection (int direction)
    {
        direction = direction;
    }

    public void DestroyBullet()
    {
        gameObject.SetActive(false);
    }
}
