using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPatrolAnimation : MonoBehaviour
{
    private Animator animator;
    void Awake()
    {
        //animationStateChanger = GetComponent<AnimationStateChanger>();
        animator=GetComponent<Animator>();
    }
    public void PlayJumpAnimation()
    {
        animator.Play("Jump");
    }
    public void PlayIdleAnimation()
    {
        animator.Play("Idle");
    }
}
