using System;
using System.Collections;
using System.Collections.Generic;
using ProjectDOOM.Weapons.Interfaces;
using TNRD;
using UnityEngine;

namespace ProjectDOOM.Weapons.V2
{
	public class Shotgun : Weapon
	{
		[Space(10)]
		public HitScanFireMethod fireMethod = new HitScanFireMethod();
		
		protected override void Awake()
		{
			base.Awake();
			eventHandler.SetWeaponComponent(this);
		}

		protected override void Update()
		{
			base.Update();
		}

		protected override void FireWeapon()
		{
			// set the origin and direction of the projectile
			fireMethod.origin = transform.position;
			fireMethod.direction = transform.forward;
			
			// fire the projectile
			fireMethod.Fire();
			
			// reduce ammo
			ammo--;
			
			// invoke the event
			InvokeFireConditionMet();
			
			// fireMethod.HitResults.collider.GetComponent<IDamageable>()?.TakeDamage(damage);
		}
	}
}