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
		public delegate void FireDelegate();
		
		public TriggerType TriggerMethod => triggerMethod;
		
		[Header("Input Parameters")]
		[SerializeField]
		private TriggerType triggerMethod = TriggerType.Single;
		
		public void Fire(FireDelegate function, KeyCode key = KeyCode.Mouse0)
		{
			switch (TriggerMethod)
			{
				// default:
				case TriggerType.Single:
					if (Input.GetKeyDown(key)) function();
					break;
				case TriggerType.Automatic:
					if (Input.GetKey(key)) function();
					break;
			}
		}
		
		public void Fire(bool[] conditions, FireDelegate function, KeyCode key = KeyCode.Mouse0)
		{
			switch (TriggerMethod)
			{
				// default:
				case TriggerType.Single:
					if (Input.GetKeyDown(key) && CheckConditionals(conditions)) function();
					break;
				case TriggerType.Automatic:
					if (Input.GetKey(key) && CheckConditionals(conditions)) function();
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
	
	// e.g., click vs hold
	public enum TriggerType
	{
		Single,
		Automatic
	}
}
