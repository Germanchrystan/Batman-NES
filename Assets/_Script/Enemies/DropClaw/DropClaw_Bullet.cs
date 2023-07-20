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
    [SerializeField] private int direction;
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
    }
    void Start()
    {
        // Setting wall check based on the direction
        wallCheck= new GameObject("WallCheck");
        wallCheck.transform.parent = this.transform;
        wallCheck.transform.localPosition = new Vector2(direction * 10, 0);
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
        if(currentState == GROUNDED && gameObject.activeSelf)
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
        else
        {
            rg.velocity = new Vector2(0f, rg.velocity.y);
        }

    }
    public void SetDirection (int setDirection)
    {
        direction = setDirection;
    }

    void OnDisable()
    {   
        currentState = DROPPING;
        groundedTimer = 1f;
    }

    void OnEnable()
    {
        currentState = DROPPING;
        groundedTimer = 1f;
    }
    public void DestroyBullet()
    {
        PowerUpSpawner.RequestPowerUp(gameObject.transform);
        gameObject.SetActive(false);
    }

    public void GetDamage(int damageAmount)
    {
        animationStateChanger.ChangeAnimationState(animator, currentState, EXPLODE);
    }
}
