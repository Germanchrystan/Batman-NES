using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLateralMovement : MonoBehaviour
{
    private Rigidbody2D rg;

    private int direction;
    [SerializeField] private LateralSpeedSO lateralSpeedSO;
    [SerializeField] private ShouldGroundCheckSO shouldGroundCheckSO;
    [SerializeField] private InitialDirectionSO initialDirectionSO;
    [SerializeField] private Flip flip;

    // Transforms
    private Transform frontCheck;
    private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    
    // Values
    private float frontCheckDistance = 5f;
    private float groundCheckRadius = 10f;

    // Booleans
    private bool frontGroundCheck;
    Vector2 rayCastDirection;
    RaycastHit2D frontHitInfo;
    private bool shouldFlip;
    private bool canMove = true;


    void OnEnable()
    {
        if(direction != 0 && direction != initialDirectionSO.direction)
        {
            flip.performFlip(false, -1);
        }
        direction = initialDirectionSO.direction;
    }
    void Awake()
    {
        rg = GetComponent<Rigidbody2D>();
        frontCheck = gameObject.transform.Find("FrontCheck").gameObject.transform;
        if(shouldGroundCheckSO.shouldGroundCheck)
        {
            groundCheck = gameObject.transform.Find("GroundCheck").gameObject.transform;
        }
    }
    void Start()
    {
        flip = GetComponent<Flip>();
    }
    void Update()
    {
       shouldFlip = ShouldFlip();
       if(shouldFlip)
       {
            flip.performFlip(false, -1); // TODO: Remove arguments in actual method
            direction *= -1;
       }
    }
    void FixedUpdate()
    {
        if(canMove)
        {
            rg.velocity = new Vector2(direction * lateralSpeedSO.speed, rg.velocity.y);
        }
        else
        {
            rg.velocity = new Vector2(0, rg.velocity.y);
        }
    }
    public bool ShouldFlip()
	{
        if(shouldGroundCheckSO.shouldGroundCheck)
        {
		    frontGroundCheck = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
        else
        {
            frontGroundCheck = true;
        }
        rayCastDirection = direction < 0 ? Vector2.left : Vector2.right;
        frontHitInfo = Physics2D.Raycast(frontCheck.position, rayCastDirection, frontCheckDistance, groundLayer);
        return !frontGroundCheck ||  frontHitInfo.collider != null;
    }
    public void InvokeStopMovement()
    {
        StartCoroutine(StopMovement());
    }
    IEnumerator StopMovement()
    {
        canMove = false;
        yield return new WaitForSeconds(1f);
        canMove = true;
    }
}
