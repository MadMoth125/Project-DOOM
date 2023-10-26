using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectDOOM.Weapons
{
	[CreateAssetMenu]
	public class GunControlParameters_SO : ScriptableObject
	{
		/// <summary>
		/// Delegate for a function that can be called when the weapon is fired.
		/// </summary>
		public delegate void FireDelegate();
		
		public TriggerType TriggerMethod => triggerMethod;
		
		[Header("Input Parameters")]
		[SerializeField]
		[Tooltip("How should the weapon be fired?\n" +
		         "\nSingle: Called for a single frame when the selected key is pressed.\n" +
		         "\nAutomatic: Called for every frame while the selected key is pressed.")]
		private TriggerType triggerMethod = TriggerType.Single;
		
		public void Fire(FireDelegate function, KeyCode key = KeyCode.Mouse0)
		{
			switch (TriggerMethod)
			{
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
				case TriggerType.Single:
					if (Input.GetKeyDown(key) && CheckConditionals(conditions)) function();
					break;
				case TriggerType.Automatic:
					if (Input.GetKey(key) && CheckConditionals(conditions)) function();
					break;
			}
		}
		
		private bool CheckConditionals( bool[] conditions)
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
