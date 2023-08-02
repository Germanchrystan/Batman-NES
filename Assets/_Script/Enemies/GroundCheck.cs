using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    // Transforms
    public Transform groundCheck;
    // Values
    private float groundCheckRadius = 10f;
    
    public LayerMask groundLayer;
    public bool frontGroundCheck;

    void Awake()
    {
        groundCheck = gameObject.transform.Find("GroundCheck").gameObject.transform;
    }

    public bool ShouldFlip(bool facingLeft)
	{
		frontGroundCheck = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        return !frontGroundCheck;
    }
}
