using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJump : MonoBehaviour
{
    public Transform frontPoint;
    private PlayerMovement PlayerMovement;

    public LayerMask groundLayer;
    public float frontPointRadius;

    bool isFacingWall;
    bool isGrounded;
    bool canWallJump;


    void Awake()
    {
        PlayerMovement = GetComponent<PlayerMovement>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isFacingWall = Physics2D.OverlapCircle(frontPoint.position, frontPointRadius, groundLayer);
        isGrounded = PlayerMovement.isGrounded;
        canWallJump = isGrounded && isFacingWall;
        if(canWallJump)
        {
            Debug.Log("CAN WALL JUMP");
        }
    }
}
