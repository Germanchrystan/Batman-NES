using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimationStateChanger))]
public class DropClaw : MonoBehaviour
{
    private Animator animator;
    private AnimationStateChanger animationStateChanger;
    private const string IDLE = "Idle";
    private const string OPEN = "Open";
    private string currentState = IDLE;
    private float timerLimit = 1f;
    private float currentTimer = 2f;

    private DropClawPool dropClawPool;
    void Awake()
    {
        animator=GetComponent<Animator>();
        animationStateChanger=GetComponent<AnimationStateChanger>();
        dropClawPool = DropClawPool.Instance;
    }
    void Update()
    {
        currentTimer -= Time.deltaTime;
        if(currentTimer <= 0f)
        {   
            currentTimer = timerLimit;
            if(currentState == IDLE){
                currentState = animationStateChanger.ChangeAnimationState(animator, currentState, OPEN);
                DropBullet();
            }
            else {
                currentState = animationStateChanger.ChangeAnimationState(animator, currentState, IDLE);
            }
        }
    }

    public void DropBullet()
    {
        GameObject bullet = dropClawPool.RequestBullet();
        if(bullet != null) bullet.transform.position = transform.position + Vector3.down * 0.5f;
    }
}
