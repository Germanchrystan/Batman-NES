using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    private bool playerFound = false;
    private bool setTarget = false;
    private bool shouldExplode = false;
    private GameObject target;
    private Vector2 playerPosition;
    private float speed=100f;
    private Rigidbody2D rg;
    // Animation
	private Animator animator;
    private string currentAnimationState;
    private AnimationStateChanger animationStateChanger;

    private const string MOVE = "Move";
    private const string EXPLODE = "Explode";
    private const string IDLE = "Idle";

    public Sprite referenceImage;
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
            if(!setTarget)
            {
                SetTarget(playerPosition);
            }
            else {
                MoveTowardsTarget();
            }
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
        playerPosition = new Vector2(playerFoundPosition.x, playerFoundPosition.y);
    }

    void drawTargetPoint(float x, float y)
    {
        target = new GameObject("Target Point");
        target.transform.position = new Vector3(x, y, 0);
        // SpriteRenderer renderer = target.AddComponent<SpriteRenderer>();
        // renderer.sprite = referenceImage;
    }

    void SetTarget(Vector2 target)
    {
        float currentYPosition = transform.position.y;
        Vector2 offset = new Vector2 (target.x, currentYPosition);
        drawTargetPoint(offset.x, offset.y);
        setTarget = true;
    }

    // TODO: Decouple to a separate script, in order to be used with other game objects (Javelin) 
    void MoveTowardsTarget() 
    {
        float currentXPosition = transform.position.x;
        Vector2 targetPosition = target.transform.position;
        //Get the difference.
        if(Math.Abs(currentXPosition - targetPosition.x) > .1f) 
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            return;
        }
        else
        {
            rg.velocity = Vector2.zero;
            Explode();
            return;
        }
    }

    private void Explode()
    {
        shouldExplode = true;
    }
}
