using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectDOOM.deprecated
{
	/// <summary>
	/// A <see cref="WeaponEventHandler"/> that handles single shot weapons
	/// </summary>
	public class SingleShotEventHandler : WeaponEventHandler
	{
		public override void EventFireStart()
		{
			base.EventFireStart();
		
			IsFiring = true;
			CanFire = false;
		}

		public override void EventFireEnd()
		{
			base.EventFireEnd();
		
			AnimatorComponent.SetBool(firePropertyStringHash, false);
		
			IsFiring = false;
			CanFire = true;
		}

		public override void EventFire()
		{
			base.EventFire();
		}

		protected override void HandleFireState()
		{
			base.HandleFireState();
		
			//AnimatorComponent.SetBool(firePropertyStringHash, IsFiring);
		}

		protected override void EventPlayerFired()
		{
			base.EventPlayerFired();
		
			if (!CanFire || IsFiring) return; // do not proceed if we cannot fire or are already firing

			AnimatorComponent.SetBool(firePropertyStringHash, true);
		}
	}
}