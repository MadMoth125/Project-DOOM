using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace ProjectDOOM.Weapons.V2
{
	public abstract class Weapon : MonoBehaviour
	{
		public TriggerMethod_SO triggerMethod;
		public FireMethod_SO fireMethod;
		public int damage;

		public bool[] fireConditionals = new bool[] { true };

		private void Update()
		{
			PostUpdate();
		}

		protected virtual void PostUpdate()
		{
			
		}
	}

	public enum FireType
	{
		HitScan,
		Projectile
	}
}