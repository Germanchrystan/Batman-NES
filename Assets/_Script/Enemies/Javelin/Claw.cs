using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour
{
    private float advanceSpeed = 100f;
    private Rigidbody2D rg;

    // Claw state
    private string currentState = IDLE_STATE;
    const string IDLE_STATE = "Idle";
    const string ADVANCE_STATE = "Advance";
    const string RETRACT_STATE = "Retract";
    // Idle timer
    private float currentTimer = 0f;
    private int currentTimerListPosition = 0;
    private float[] timerList = { 1f,.5f,.5f,.15f,.15f };
    // Advance Timer
    private float currentAdvanceTimer = 0f;
    private float advanceTimer = .5f;
    public Transform startPoint;
    // Facing direction
    public bool facingLeft;
    private int direction;
    // Endpoint
    public Transform endpoint;

    void Awake()
    {
        facingLeft = true;
        direction = facingLeft ? -1 : 1;
        rg = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(currentState == IDLE_STATE)
        {
            currentTimer -= Time.deltaTime;
            if(currentTimer <= 0f)
            {
                SetNewTimer();
                SetState(ADVANCE_STATE);
                currentAdvanceTimer = advanceTimer;
            }
        }
        // Advance
        if (currentState == ADVANCE_STATE)
        {
            currentAdvanceTimer -= Time.deltaTime;
            if(currentAdvanceTimer <= 0f)
            {
                currentAdvanceTimer = advanceTimer;
                SetState(RETRACT_STATE);
            }
        }

        if (currentState == RETRACT_STATE)
        {
            currentAdvanceTimer -= Time.deltaTime;
            if(currentAdvanceTimer <= 0f)
            {
                currentAdvanceTimer = 0;
                SetState(IDLE_STATE);
            }
        }
    }

    void FixedUpdate()
    {
        if(currentState == ADVANCE_STATE)
        {
			rg.velocity = new Vector2(direction * advanceSpeed, rg.velocity.y);
        }
        else if (currentState == RETRACT_STATE)
        {
            if (Math.Abs(endpoint.position.x - transform.position.x) > .2f) {
                rg.velocity = new Vector2((- 1) * (direction) * advanceSpeed, rg.velocity.y);
            }
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
}
