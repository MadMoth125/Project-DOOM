using System;
using System.Collections;
using System.Collections.Generic;
using ProjectDOOM.Weapons.Interfaces;
using TNRD;
using UnityEngine;
using UnityEngine.Events;

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
		
		protected IWeaponEventHandler eventHandler;
		
		[Header("Weapon Parameters")]
		public TriggerType triggerType;
		public ConditionalTrigger triggerMethod;
		
		[HideInInspector]
		public int ammo;
		public int maxAmmo;
		public int damage;
		
		public bool CanPickupAmmo => ammo < maxAmmo;
		public bool CanFire => ammo > 0;

		[HideInInspector]
		// An array of booleans that are looped through when the fire input
		// is pressed. If all of the conditions are met, the weapon will fire.
		public bool[] fireConditionals;
		
		protected virtual void Awake()
		{
			fireConditionals = new bool[] { true, true };
			
			eventHandler = GetComponentInChildren<IWeaponEventHandler>();
			
			switch (triggerType)
			{
				case TriggerType.Single:
					triggerMethod = new ManualTrigger();
					break;
				case TriggerType.Automatic:
					triggerMethod = new AutomaticTrigger();
					break;
				default:
					break;
			}
		}

		protected virtual void Update()
		{
			fireConditionals[0] = eventHandler.CanFire; // is anim finished
			fireConditionals[1] = CanFire; // do we have ammo
			
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
		
		public virtual void AddAmmo(int amount)
		{
			ammo = Mathf.Clamp(ammo + amount, 0, maxAmmo);
		}
		
		public virtual void AddAmmo(int amount, out int overflow)
		{
			// if the ammo + amount is greater than the max ammo, set the remaining ammo to the difference
			if (ammo + amount > maxAmmo)
			{
				overflow = ammo + amount - maxAmmo;
			}
			else
			{
				overflow = 0;
			}
			
			ammo = Mathf.Clamp(ammo + amount, 0, maxAmmo);
		}
		
	}

	public enum FireType
	{
		HitScan,
		Projectile
	}
}