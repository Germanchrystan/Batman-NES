using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nighslayer : MonoBehaviour
{
    private Rigidbody2D rg;
    private DetectPlayerOnFront detectPlayerOnFront;
    private LateralPatrolling lateralPatrolling;
    private EnemyHealth enemyHealth;
    private bool playerDetected = false;
    private bool isAlive = true;

    // Animation
    private Animator animator;
    private string currentAnimationState;

    void Awake()
    {  
        rg=GetComponent<Rigidbody2D>();
        animator=GetComponent<Animator>();
        detectPlayerOnFront=GetComponent<DetectPlayerOnFront>();
        lateralPatrolling=GetComponent<LateralPatrolling>();
        enemyHealth=GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        isAlive = enemyHealth.currentHealth > 0;
        playerDetected = detectPlayerOnFront.DetectPlayer();
        if(!playerDetected && isAlive)
        {
            lateralPatrolling._Update();
        }
    }

    void FixedUpdate()
    {   
        if(!playerDetected && isAlive)
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
        if(!isAlive)
        {
            newAnimationState = "Death";
            ChangeAnimationState(newAnimationState);
            return;
        }
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
