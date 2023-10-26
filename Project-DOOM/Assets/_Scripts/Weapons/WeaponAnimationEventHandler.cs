using System;
using System.Collections;
using System.Collections.Generic;
using ProjectDOOM.Weapons.Interfaces;
using ProjectDOOM.Weapons.V2;
using UnityEngine;
using Utilities;

namespace ProjectDOOM.Weapons
{
    /// <summary>
    /// Make sure the script execution order is set to run this script AFTER the <see cref="Weapon"/> script.
    /// This is so the handler can properly subscribe to the <see cref="Weapon.OnFireConditionMet"/> event.
    /// </summary>
    public class WeaponAnimationEventHandler : MonoBehaviour, IWeaponEventHandler
    {
    	/// <summary>
    	/// Translates the string "Fire" to a hash code for the Animator.
    	/// Every weapon state machine should have a Fire condition.
    	/// </summary>
    	public static readonly int FireConditionHash = Animator.StringToHash("Fire");
    	
    	/// <summary>
    	/// Animator component that the event handler will interact with.
    	/// Should be on the same GameObject as this script.
    	/// </summary>
    	public Animator AnimatorComponent { get; set; }
    	
    	/// <summary>
    	/// Weapon component that the event handler will interface with.
    	/// Should be set in the Awake method of the Weapon that uses this event handler.
    	/// </summary>
    	public IWeapon WeaponComponent { get; private set; }
    
    	/// <summary>
    	/// A flag that determines if the weapon can fire.
    	/// </summary>
    	public bool CanFire { get; private set; }
    
    	protected virtual void Awake() => AnimatorComponent = gameObject.SearchForComponent<Animator>();
    	
    	protected virtual void OnEnable() => WeaponComponent.OnFireConditionMet += FireWeaponAnimation;
    	
    	protected virtual void OnDisable() => WeaponComponent.OnFireConditionMet -= FireWeaponAnimation;
    
    	public void SetWeaponComponent(IWeapon weapon) => WeaponComponent = weapon;
    
    	public virtual void EnableWeapon() => CanFire = true;
    	
    	public virtual void DisableWeapon() => CanFire = false;
    
    	protected virtual void FireWeaponAnimation() => AnimatorComponent.SetBool(FireConditionHash, true);
    }
}