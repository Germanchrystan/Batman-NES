using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour
{
    public float advanceSpeed = 100f;
    public float retractSpeed = 200f;
    private Rigidbody2D rg;

    // Claw state
    private string currentState = IDLE_STATE;
    const string IDLE_STATE = "Idle";
    const string ADVANCE_STATE = "Advance";
    const string RETRACT_STATE = "Retract";
    // Idle timer
    private float currentTimer = 0f;
    private int currentTimerListPosition = 0;
    private float[] timerList = { 10f,10f,10f,2f,2f };
    // Advance Timer
    private float currentAdvanceTimer = 0f;
    private float advanceTimer = 3f;
    public Transform startPoint;
    // Facing direction
    public bool facingLeft;
    private int direction;

    void Awake()
    {
        direction = facingLeft ? -1 : 1;
        rg = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(currentState == IDLE_STATE)
        {
            currentTimer -= Time.deltaTime;
        }
        if(currentTimer <= 0f)
        {
            SetNewTimer();
            SetState(ADVANCE_STATE);
            currentAdvanceTimer = advanceTimer;
        }
        // Advance
        if (currentState == ADVANCE_STATE)
        {
            currentAdvanceTimer -= Time.deltaTime;
            if(currentAdvanceTimer <= 0f)
            {
                SetState(RETRACT_STATE);
                currentAdvanceTimer = 0f;
            }
        }

        if (currentState == RETRACT_STATE)
        {
            if(Math.Abs(transform.position.x - startPoint.position.x) > .1f)
            {
                SetState(IDLE_STATE);
            } 
        }
    }

    void FixedUpdate()
    {
        if(currentState == ADVANCE_STATE)
        {
			rg.velocity += new Vector2(direction * advanceSpeed, rg.velocity.y) * Time.deltaTime;
        }
        else if (currentState == RETRACT_STATE)
        {
            rg.velocity += new Vector2((- 1) * (direction) * retractSpeed, rg.velocity.y) * Time.deltaTime;
        }
        else
        {
            rg.velocity = Vector2.zero;
        }
    }

    void SetState (string newState)
    {
        currentState = newState;
    }

    private void SetNewTimer()
    {
        if (currentTimerListPosition + 1 == timerList.Length)
        {
            currentTimerListPosition = 0;
        }
        else
        {
            currentTimerListPosition++;
        }
        currentTimer = timerList[currentTimerListPosition];
    }

    void Advance()
    {
        
    }
}
