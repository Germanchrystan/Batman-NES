using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nighslayer : MonoBehaviour
{
    private Rigidbody2D rg;
    private DetectPlayerOnFront detectPlayerOnFront;
    private LateralPatrolling lateralPatrolling;

    private bool playerDetected = false;

    // Animation
    private Animator animator;
    private string currentAnimationState;

    void Awake()
    {  
        rg=GetComponent<Rigidbody2D>();
        animator=GetComponent<Animator>();
        detectPlayerOnFront=GetComponent<DetectPlayerOnFront>();
        lateralPatrolling=GetComponent<LateralPatrolling>();
    }

    // Update is called once per frame
    void Update()
    {
        playerDetected = detectPlayerOnFront.DetectPlayer();
        if(!playerDetected)
        {
            lateralPatrolling._Update();
        }
    }

    void FixedUpdate()
    {   
        if(!playerDetected)
        //if(!playerDetected && currentAnimationState != "Attack")
        {
            lateralPatrolling._FixedUpdate();
        }
        else
        {
            rg.velocity = new Vector2(0,0);
        }
    }

    void LateUpdate()
    {
        string newAnimationState;
        if(!playerDetected)
        {
            newAnimationState = "Walking";
        }
        else
        {
            newAnimationState = "Attack";
        }
        ChangeAnimationState(newAnimationState);
    }

    //TODO take this function into its own script
    void ChangeAnimationState(string newAnimationState)
	{
		if (currentAnimationState == newAnimationState) return;

		animator.Play(newAnimationState);

		currentAnimationState = newAnimationState;
	}
}
