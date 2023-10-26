using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

namespace ProjectDOOM.Weapons.V2
{
	public abstract class Weapon : MonoBehaviour, IWeapon
	{
		/// <summary>
		/// Meant for allowing other scripts to react to the weapon firing
		/// without having to subscribe to the OnFireConditionMet event.
		/// </summary>
		public UnityEvent onFire;
		
		/// <summary>
		/// Invoked when the weapon has fired.
		/// </summary>
		public event Action OnFireConditionMet;
		
		[Header("Weapon Parameters")]
		public TriggerType triggerType;
		public ConditionalTrigger triggerMethod;
		
		public int damage;
		public int maxAmmo;
		
		[HideInInspector]
		public int ammo;
		[HideInInspector]
		// An array of booleans that are looped through when the fire input
		// is pressed. If all of the conditions are met, the weapon will fire.
		public bool[] fireConditionals = new bool[] { true };

		protected virtual void Awake()
		{
			switch (triggerType)
			{
				case TriggerType.Single:
					triggerMethod = new ManualTrigger();
					break;
				case TriggerType.Automatic:
					triggerMethod = new AutomaticTrigger();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		protected virtual void Update()
		{
			triggerMethod.ShouldFire(FireWeapon, fireConditionals);
		}

		protected abstract void FireWeapon();
		
		/// <summary>
		/// A dedicated method for invoking both the C# and Unity OnFire events.
		/// </summary>
		protected void InvokeFireConditionMet()
		{
			OnFireConditionMet?.Invoke();
			onFire?.Invoke();
		}
	}

	public enum FireType
	{
		HitScan,
		Projectile
	}
}