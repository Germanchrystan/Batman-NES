using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontCheck : MonoBehaviour
{
    // Transforms
    public Transform frontCheck;
    // Values
    private float frontCheckDistance = 5f;
    // Proyections
    Vector2 rayCastDirection;
    RaycastHit2D frontHitInfo; 
    
    public LayerMask groundLayer;

    void Awake()
    {
        frontCheck = gameObject.transform.Find("FrontCheck").gameObject.transform;
    }

    public bool ShouldFlip(bool facingLeft)
	{
        rayCastDirection = facingLeft ? Vector2.left : Vector2.right;
        frontHitInfo = Physics2D.Raycast(frontCheck.position, rayCastDirection, frontCheckDistance, groundLayer);
        return frontHitInfo.collider != null;
    }
}
