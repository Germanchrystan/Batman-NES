using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    private bool playerFound = false;
    private bool shouldExplode = false;
    private Vector2 playerPosition;
    public float speed=10f;
    private Rigidbody2D rg;
    
    // Animation
	private Animator animator;
    private string currentAnimationState;
    private AnimationStateChanger animationStateChanger;

    private const string MOVE = "Move";
    private const string EXPLODE = "Explode";
    private const string IDLE = "Idle";
    void Awake()
    {
        rg=GetComponent<Rigidbody2D>();
        animator=GetComponent<Animator>();
        animationStateChanger=GetComponent<AnimationStateChanger>();
    }

    public void FixedUpdate()
    {
        if(playerFound && !shouldExplode)
        {
            MoveTowardsTarget(playerPosition);
        }
        else
        {
            rg.velocity = new Vector2(0,0);
        }
    }

    public void LateUpdate()
    {
        string newAnimationState;
        if(shouldExplode)
        {
            currentAnimationState = animationStateChanger.ChangeAnimationState(animator, currentAnimationState, EXPLODE);
            return;
        }
        else if(playerFound)
        {
            currentAnimationState = animationStateChanger.ChangeAnimationState(animator, currentAnimationState, MOVE);
            return;
        }
        currentAnimationState = animationStateChanger.ChangeAnimationState(animator, currentAnimationState, IDLE);
    }

    public void PlayerFound(Vector2 playerFoundPosition)
    {
        playerFound = true;
        playerPosition = new Vector2(playerFoundPosition.x, this.transform.position.y);
    }

    void MoveTowardsTarget(Vector2 target) 
    {
        Vector2 currentPosition = transform.position;
        Vector2 offset = target - currentPosition;
        //Get the difference.
        if(offset.magnitude > .1f) 
        {
            offset = offset.normalized * speed;
            rg.velocity = offset * Time.deltaTime;
        }
        else
        {
            Explode();
        }
    }

    private void Explode()
    {
        shouldExplode = true;
    }
}
