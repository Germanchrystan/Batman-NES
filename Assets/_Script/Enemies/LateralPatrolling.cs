using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateralPatrolling : MonoBehaviour
{
    // General
	private Rigidbody2D rg;
	private Animator animator;
	public float movePause = .1f;

    // Movement
    public float speed=10f;
	private bool facingLeft = true;
    private int direction = -1;

    public bool shouldFlip;

    private CheckSurrounding checkSurrounding;
    private Flip flip;

    void Awake()
    {
        rg=GetComponent<Rigidbody2D>();
        animator=GetComponent<Animator>();
        // enemyHealth=GetComponent<EnemyHealth>();
        checkSurrounding=GetComponent<CheckSurrounding>();
        flip=GetComponent<Flip>();
    }

    // Update is called once per frame
    void Update()
    {
        shouldFlip=checkSurrounding.ShouldFlip(facingLeft);
        if(shouldFlip)
        {
            facingLeft = flip.performFlip(facingLeft, direction);
            direction = facingLeft ? -1 : 1;
        }
    }

    void FixedUpdate()
    {
        rg.velocity = new Vector2(speed * direction, rg.velocity.y);
    }
}
