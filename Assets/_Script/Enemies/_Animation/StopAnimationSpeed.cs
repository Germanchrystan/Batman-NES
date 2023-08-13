using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAnimationSpeed : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator=GetComponent<Animator>();
    }
    
	public void InvokeStopAnimation(float timer)
	{
		StartCoroutine(StopAnimation(timer));
	}
	IEnumerator StopAnimation(float timer)
	{
        animator.speed = 0;
		yield return new WaitForSeconds(timer);
        animator.speed = 1;
	}
}
