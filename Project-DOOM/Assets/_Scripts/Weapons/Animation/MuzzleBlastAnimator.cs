using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace ProjectDOOM.Weapons.V2
{
	public class MuzzleBlastAnimator : MonoBehaviour
	{
		[Tooltip("The name of the animation to play.\n" +
		         "Please reference the file name in the assets window.")]
		public string targetAnimationName;
		public Animator animatorComponent;

		private void Awake()
		{
			animatorComponent = GetComponent<Animator>();
		}

		public void PlayAnimation()
		{
			animatorComponent.Play(targetAnimationName);
		}
	}
}
