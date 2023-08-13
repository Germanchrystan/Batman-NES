using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDespawn : MonoBehaviour
{
    private const float TIMER = 1f;
    private float notVisibleTimer = TIMER;

    private bool visible = true;

    void OnEnable()
    {
        notVisibleTimer = 1f;
    }

    void Update()
    {
        if(!visible)
        {
            notVisibleTimer -= Time.deltaTime;
            if (notVisibleTimer <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    void OnBecameInvisible()
    {
        
        visible = false;
    }

    void OnBecameVisible()
    {
        notVisibleTimer = TIMER;
        visible=true;
    }

}
