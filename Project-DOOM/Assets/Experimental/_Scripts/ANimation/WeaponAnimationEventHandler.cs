using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationEventHandler : MonoBehaviour, IWeaponEventHandler
{
	/// <summary>
	/// Translates the string "Fire" to a hash code for the Animator.
	/// Every weapon state machine should have a Fire condition.
	/// </summary>
	public static readonly int FireConditionHash = Animator.StringToHash("Fire");
	
	/// <summary>
	/// Animator component that the event handler will interface with.
	/// </summary>
	public Animator AnimatorComponent { get; set; }
	public Weapon WeaponComponent { get; set; }

	/// <summary>
	/// A flag that determines if the weapon can fire.
	/// </summary>
	public bool CanFire { get; private set; }

	protected virtual void Awake()
	{
		// Animator should be on the same GameObject as this script
		AnimatorComponent ??= GetComponent<Animator>();
		
		/*WeaponComponent ??= GetComponentInParent<Weapon>(); // check if a weapon in in the parent GameObject
		WeaponComponent ??= GetComponent<Weapon>(); // check if a weapon is in the same GameObject
		WeaponComponent ??= GetComponentInChildren<Weapon>(); // check if a weapon is in a child GameObject*/
	}
	
	protected virtual void OnEnable()
	{
		if (WeaponComponent)
		{
			WeaponComponent.OnFireConditionMet += FireWeaponAnimation;
		}
	}
	
	protected virtual void OnDisable()
	{
		if (WeaponComponent)
		{
			WeaponComponent.OnFireConditionMet -= FireWeaponAnimation;
		}
	}

	public virtual void EnableWeapon()
	{
		CanFire = true;
	}
	
	public virtual void DisableWeapon()
	{
		CanFire = false;
	}

	protected virtual void FireWeaponAnimation()
	{
		AnimatorComponent.SetBool(FireConditionHash, true);
	}
}