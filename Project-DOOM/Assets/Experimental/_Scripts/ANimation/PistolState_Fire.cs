using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolState_Fire : Weapon_StateMachine
{
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, stateInfo, layerIndex);
		// eventHandler.DisableWeapon();
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateUpdate(animator, stateInfo, layerIndex);
		// if (stateInfo.normalizedTime >= 0.9f)
		// {
		// 	animator.SetBool("Fire", false);
		// }
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateExit(animator, stateInfo, layerIndex);
		
		animator.SetBool("Fire", false);
	}
}