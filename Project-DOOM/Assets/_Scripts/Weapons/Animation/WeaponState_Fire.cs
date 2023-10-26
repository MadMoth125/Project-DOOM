using System;
using System.Collections;
using System.Collections.Generic;
using ProjectDOOM.Weapons.StateMachine;
using UnityEngine;

namespace ProjectDOOM.Weapons.StateMachine
{
	/// <summary>
	/// Class contains the very basics of the "Fire" state for weapons.
	/// Most weapons are simple enough to use this class as is.
	/// </summary>
	public class WeaponState_Fire : Weapon_StateMachine
	{
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateUpdate(animator, stateInfo, layerIndex);
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateExit(animator, stateInfo, layerIndex);
		
			animator.SetBool(WeaponAnimationEventHandler.FireConditionHash, false);
		}
	}
}
