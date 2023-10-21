using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Attempt2
{
	[CreateAssetMenu]
	public class GunParameters_SO1 : ScriptableObject
	{
		public event Action OnGunFired;
		
		[Header("Input Parameters")]
		[SerializeField]
		private TriggerType triggerMethod = TriggerType.Single;
		
		[Header("Gun Parameters")]
		[SerializeField]
		private int damage = 10;
		[Space]
		[SerializeField]
		private int ammoCount = 10;
		[SerializeField]
		private int maxAmmoCount = 100;
		
		public TriggerType TriggerMethod => triggerMethod;

		public void Fire(KeyCode key = KeyCode.Mouse0)
		{
			switch (TriggerMethod)
			{
				// default:
				case TriggerType.Single:
					if (Input.GetKeyDown(key)) OnGunFired?.Invoke();
					break;
				case TriggerType.Automatic:
					if (Input.GetKey(key)) OnGunFired?.Invoke();
					break;
			}
		}
		
		public void Fire(bool[] conditions, KeyCode key = KeyCode.Mouse0)
		{
			switch (TriggerMethod)
			{
				// default:
				case TriggerType.Single:
					if (Input.GetKeyDown(key) && CheckConditionals(conditions)) OnGunFired?.Invoke();
					break;
				case TriggerType.Automatic:
					if (Input.GetKey(key) && CheckConditionals(conditions)) OnGunFired?.Invoke();
					break;
			}
		}
		
		private bool CheckConditionals([CanBeNull] bool[] conditions)
		{
			if (conditions == null || conditions.Length == 0) return true;

			foreach (bool condition in conditions)
			{
				// result &= condition; old implementation
				// if any condition is false, early return false
				if (!condition) return false;
			}
			
			// if loop completes, all conditions must be true
			return true;
		}
	}
	
	// e.g., pistol vs rifle
	public enum TriggerType
	{
		Single,
		Automatic
	}
}
