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
		[SerializeField]
		protected SerializableInterface<IWeaponEventHandler> eventHandler;
		
		[Space(10)]
		public HitScanFireMethod fireMethod = new HitScanFireMethod();
		
		protected override void Awake()
		{
			base.Awake();
			eventHandler.Value.SetWeaponComponent(this);
		}

		protected override void Update()
		{
			fireConditionals[0] = eventHandler.Value.CanFire;
			base.Update();
		}

		protected override void FireWeapon()
		{
			fireMethod.origin = transform.position;
			fireMethod.direction = transform.forward;
			
			fireMethod.Fire();

			InvokeFireConditionMet();
			
			// fireMethod.HitResults.collider.GetComponent<IDamageable>()?.TakeDamage(damage);
		}
	}
}