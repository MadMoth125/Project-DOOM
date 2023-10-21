using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationEventHandler : MonoBehaviour
{
	public static readonly int FireConditionHash = Animator.StringToHash("Fire");
	
	public Animator AnimatorComponent { get; private set; }
	public Weapon WeaponComponent { get; private set; }

	public bool CanFire { get; private set; }

	protected virtual void Awake()
	{
		// Animator should be on the same GameObject as this script
		AnimatorComponent ??= GetComponent<Animator>();
		
		WeaponComponent ??= GetComponentInParent<Weapon>(); // check if a weapon in in the parent GameObject
		WeaponComponent ??= GetComponent<Weapon>(); // check if a weapon is in the same GameObject
		WeaponComponent ??= GetComponentInChildren<Weapon>(); // check if a weapon is in a child GameObject
	}
	
	protected virtual void OnEnable()
	{
		WeaponComponent.OnFireConditionMet += FireWeaponAnimation;
	}
	
	protected virtual void OnDisable()
	{
		WeaponComponent.OnFireConditionMet -= FireWeaponAnimation;
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