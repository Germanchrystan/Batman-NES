using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CheckSurrounding : MonoBehaviour
{
    // Transforms
    public Transform frontCheck;
    public Transform groundCheck;
    // Values
    private float frontCheckDistance = 5f;
    private float groundCheckRadius = 10f;
    // Proyections
    Vector2 rayCastDirection;
    RaycastHit2D frontHitInfo; 
    
    public LayerMask groundLayer;
    public bool frontGroundCheck;

    [SerializeField] private UnityEvent NotGroundedEvent;
    private bool isGrounded;

    void Awake()
    {
        frontCheck = gameObject.transform.Find("FrontCheck").gameObject.transform;
        groundCheck = gameObject.transform.Find("GroundCheck").gameObject.transform;
    }
    void Update()
    {
        IsGroundedCheck();
    } 
    public void IsGroundedCheck()
    {
        bool newIsGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if(isGrounded && !newIsGrounded)
        {
            NotGroundedEvent.Invoke();
        }
    }
    public bool ShouldFlip(bool facingLeft)
	{
		frontGroundCheck = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        rayCastDirection = facingLeft ? Vector2.left : Vector2.right;
        frontHitInfo = Physics2D.Raycast(frontCheck.position, rayCastDirection, frontCheckDistance, groundLayer);
        return !frontGroundCheck ||  frontHitInfo.collider != null;
    }
}