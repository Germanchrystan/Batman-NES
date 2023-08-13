using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateChanger : MonoBehaviour
{
    public string ChangeAnimationState(Animator animator, string currentAnimationState, string newAnimationState)
	{
		if (currentAnimationState == newAnimationState) return currentAnimationState;
		animator.Play(newAnimationState);
		return newAnimationState;
	}

}
